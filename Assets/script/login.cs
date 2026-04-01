using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UserGameData
{
    public int gold;
    public int rod;
    public int lure;
}

[System.Serializable]
public class UserCaughtFishes
{
    public int bream;
    public int catfish;
    public int ray;
    public int octopus;
}

[System.Serializable]
public class UserDataResponse
{
    public int id;
    
    // Nested data from API (Laravel converts relation 'caughtFishes' to snake_case or camelCase depending on config, but latest log showed game and caught_fishes might be present if updated correctly)
    public UserGameData game;
    
    public UserCaughtFishes caughtFishes; // Fallback for camelCase
    public UserCaughtFishes caught_fishes; // Fallback for snake_case

    // Fallback/flat fields
    public int gold;
    public int rod;
    public int bream;
    public int catfish;
    public int ray;
    public int octopus;
    public int lure;
}

[System.Serializable]
public class LoginResponse
{
    public string message;
    public UserDataResponse user;
    public string token;
}

public class login : MonoBehaviour
{
    public TextMeshProUGUI error;
    public TMP_InputField User;
    public TMP_InputField Password;
    public GameManager gameManager; // Referencia a GameManagerre
    public string apiBaseUrl = "http://127.0.0.1:8000/api"; //api base

    void Start()
    {
        if (error != null) error.gameObject.SetActive(false);
    }

    bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    }

    void ShowError(string msg)
    {
        error.text = msg;
        error.gameObject.SetActive(true);
    }

    public void LoginButton()
    {
        string email = User.text.Trim();
        string password = Password.text;

        //Validáció

        if (email == "test@email.com" && password == "testing")
        {
            error.gameObject.SetActive(false);

            //load integers (load save) from dataBase (User->gold, rod, bream, lure)

            PlayerPrefs.SetString("token", "test_token");
            PlayerPrefs.SetInt("user_id", 1);
            PlayerPrefs.Save();

            if (gameManager != null)
            {
                gameManager.userId = 1; //test user ID
                gameManager.gold = 9999;
                gameManager.rod = 0;
                gameManager.bream = 0;
                gameManager.catfish = 0;
                gameManager.ray = 0;
                gameManager.octopus = 0;
                gameManager.lure = 10;
                gameManager.bearerToken = "test_token";
            }

            Debug.Log("gold: " + gameManager.gold);
            SceneManager.LoadScene("mainMenu");
            return;
        }

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("Töltsd ki az emailt és a jelszót!");
            return;
        }

        if (!IsValidEmail(email))
        {
            ShowError("Hibás email formátum!");
            return;
        }
        if (password.Length < 6)
        {
            ShowError("A jelszónak minimum 6 karakternek kell lennie!");
            return;
        }

        error.gameObject.SetActive(false);
        StartCoroutine(LoginCoroutine(email, password));
    }

    IEnumerator LoginCoroutine(string email, string password)
    {
        string url = apiBaseUrl + "/login";
        Debug.Log("LOGIN URL: " + url);

        string json = $"{{\"email\":\"{EscapeJson(email)}\",\"password\":\"{EscapeJson(password)}\"}}";

        using (UnityWebRequest req = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(bodyRaw);
            req.downloadHandler = new DownloadHandlerBuffer();

            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Accept", "application/json");

            yield return req.SendWebRequest();

            // Hálózati error
            if (req.result != UnityWebRequest.Result.Success)
            {
                ShowError("Hálózati error: " + req.error);
                Debug.Log("BODY: " + req.downloadHandler.text);
                yield break;
            }

            // Backend error
            if (req.responseCode < 200 || req.responseCode >= 300)
            {
                ShowError("Hibás bejelentkezés!");
                Debug.Log("STATUS: " + req.responseCode);
                Debug.Log("BODY: " + req.downloadHandler.text);
                yield break;
            }

            string responseText = req.downloadHandler.text;
            Debug.Log("LOGIN RESPONSE BODY: " + responseText);
            LoginResponse data = JsonUtility.FromJson<LoginResponse>(responseText);

            if (data == null || string.IsNullOrEmpty(data.token))
            {
                ShowError("Nem jött token a szervertől!");
                Debug.Log("RESPONSE: " + responseText);
                yield break;
            }

            PlayerPrefs.SetString("token", data.token);
            if (data.user != null)
            {
                PlayerPrefs.SetInt("user_id", data.user.id);
            }
            PlayerPrefs.Save();

            Debug.Log("Sikeres login, token elmentve: " + data.token);

            // load integers directly from login payload
            if (data.user != null)
            {
                int gold = data.user.game != null ? data.user.game.gold : data.user.gold;
                int rod = data.user.game != null ? data.user.game.rod : data.user.rod;
                int lure = data.user.game != null ? data.user.game.lure : data.user.lure;

                // Handle both snake_case and camelCase serialization from Laravel
                UserCaughtFishes fishes = data.user.caught_fishes != null ? data.user.caught_fishes : data.user.caughtFishes;

                int bream = fishes != null ? fishes.bream : data.user.bream;
                int catfish = fishes != null ? fishes.catfish : data.user.catfish;
                int ray = fishes != null ? fishes.ray : data.user.ray;
                int octopus = fishes != null ? fishes.octopus : data.user.octopus;

                if (GameManager.Instance != null)
                {
                    GameManager.Instance.userId = data.user.id;
                    GameManager.Instance.gold = gold;
                    GameManager.Instance.rod = rod;
                    GameManager.Instance.bream = bream;
                    GameManager.Instance.catfish = catfish;
                    GameManager.Instance.ray = ray;
                    GameManager.Instance.octopus = octopus;
                    GameManager.Instance.lure = lure;
                    GameManager.Instance.bearerToken = data.token;
                }
                else if (gameManager != null)
                {
                    gameManager.userId = data.user.id;
                    gameManager.gold = gold;
                    gameManager.rod = rod;
                    gameManager.bream = bream;
                    gameManager.catfish = catfish;
                    gameManager.ray = ray;
                    gameManager.octopus = octopus;
                    gameManager.lure = lure;
                    gameManager.bearerToken = data.token;
                }
                Debug.Log("User data loaded successfully directly from login.");
            }

            Debug.Log("gold: " + (GameManager.Instance != null ? GameManager.Instance.gold : gameManager.gold));

            SceneManager.LoadScene("mainMenu");
        }
    }

    IEnumerator FetchUserData(string token)
    {
        string url = apiBaseUrl + "/user"; // A szerveren lévő végpont, ami visszaadja a usert
        
        Debug.Log("FETCH USER URL: " + url);
        
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.SetRequestHeader("Authorization", "Bearer " + token);
            req.SetRequestHeader("Accept", "application/json");

            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;
                UserDataResponse userData = JsonUtility.FromJson<UserDataResponse>(json);

                if (userData != null)
                {
                    PlayerPrefs.SetInt("user_id", userData.id);
                    PlayerPrefs.Save();

                    int gold = userData.game != null ? userData.game.gold : userData.gold;
                    int rod = userData.game != null ? userData.game.rod : userData.rod;
                    int lure = userData.game != null ? userData.game.lure : userData.lure;

                    // Handle both snake_case and camelCase serialization from Laravel
                    UserCaughtFishes fishes = userData.caught_fishes != null ? userData.caught_fishes : userData.caughtFishes;

                    int bream = fishes != null ? fishes.bream : userData.bream;
                    int catfish = fishes != null ? fishes.catfish : userData.catfish;
                    int ray = fishes != null ? fishes.ray : userData.ray;
                    int octopus = fishes != null ? fishes.octopus : userData.octopus;

                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.userId = userData.id;
                        GameManager.Instance.gold = gold;
                        GameManager.Instance.rod = rod;
                        GameManager.Instance.bream = bream;
                        GameManager.Instance.catfish = catfish;
                        GameManager.Instance.ray = ray;
                        GameManager.Instance.octopus = octopus;
                        GameManager.Instance.lure = lure;
                        GameManager.Instance.bearerToken = token;
                    }
                    else if (gameManager != null)
                    {
                        gameManager.userId = userData.id;
                        gameManager.gold = gold;
                        gameManager.rod = rod;
                        gameManager.bream = bream;
                        gameManager.catfish = catfish;
                        gameManager.ray = ray;
                        gameManager.octopus = octopus;
                        gameManager.lure = lure;
                        gameManager.bearerToken = token;
                    }

                    Debug.Log("User data loaded successfully.");
                }
            }
            else
            {
                Debug.LogWarning("Failed to fetch user data: " + req.error);
                Debug.LogWarning("Response Code: " + req.responseCode);
                if (req.downloadHandler != null)
                {
                    Debug.LogWarning("Response Body: " + req.downloadHandler.text);
                }
            }
        }
    }

    string EscapeJson(string s)
    {
        if (s == null) return "";
        return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
