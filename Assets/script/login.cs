using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UserDataResponse
{
    public int id;
    public int penz;
    public int bot;
    public int halak;
    public int csalik;
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
    public TextMeshProUGUI hiba;
    public TMP_InputField Felhasznalo;
    public TMP_InputField Jelszo;
    public GameManager gameManager; // Referencia a GameManagerre
    public string apiBaseUrl = "http://127.0.0.1:8000/api"; //api base

    void Start()
    {
        if (hiba != null) hiba.gameObject.SetActive(false);
    }

    bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    }

    void ShowError(string msg)
    {
        hiba.text = msg;
        hiba.gameObject.SetActive(true);
    }

    public void LoginButton()
    {
        string email = Felhasznalo.text.Trim();
        string password = Jelszo.text;

        //Validáció

        if (email == "test@email.com" && password == "testing")
        {
            hiba.gameObject.SetActive(false);

            //load integers (load save) from dataBase (Felhasznalo->penz, bot, halak, csalik)

            PlayerPrefs.SetString("token", "test_token");
            PlayerPrefs.SetInt("user_id", 1);
            PlayerPrefs.Save();

            if (gameManager != null)
            {
                gameManager.userId = 1; //test user ID
                gameManager.penz = 9999;
                gameManager.bot = 1;
                gameManager.halak = 0;
                gameManager.csalik = 10;
                gameManager.bearerToken = "test_token";
            }

            Debug.Log("penz: " + gameManager.penz);
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

        hiba.gameObject.SetActive(false);
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

            // Hálózati hiba
            if (req.result != UnityWebRequest.Result.Success)
            {
                ShowError("Hálózati hiba: " + req.error);
                Debug.Log("BODY: " + req.downloadHandler.text);
                yield break;
            }

            // Backend hiba
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
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.userId = data.user.id;
                    GameManager.Instance.penz = data.user.penz;
                    GameManager.Instance.bot = data.user.bot;
                    GameManager.Instance.halak = data.user.halak;
                    GameManager.Instance.csalik = data.user.csalik;
                    GameManager.Instance.bearerToken = data.token;
                }
                else if (gameManager != null)
                {
                    gameManager.userId = data.user.id;
                    gameManager.penz = data.user.penz;
                    gameManager.bot = data.user.bot;
                    gameManager.halak = data.user.halak;
                    gameManager.csalik = data.user.csalik;
                    gameManager.bearerToken = data.token;
                }
                Debug.Log("User data loaded successfully directly from login.");
            }

            Debug.Log("penz: " + (GameManager.Instance != null ? GameManager.Instance.penz : gameManager.penz));

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

                    if (gameManager != null)
                    {
                        gameManager.userId = userData.id;
                        gameManager.penz = userData.penz;
                        gameManager.bot = userData.bot;
                        gameManager.halak = userData.halak;
                        gameManager.csalik = userData.csalik;
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
