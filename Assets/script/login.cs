using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoginResponse
{
    public string token;
}

public class login : MonoBehaviour
{
    public TextMeshProUGUI hiba;
    public TMP_InputField Felhasznalo;
    public TMP_InputField Jelszo;

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
            LoginResponse data = JsonUtility.FromJson<LoginResponse>(responseText);

            if (data == null || string.IsNullOrEmpty(data.token))
            {
                ShowError("Nem jött token a szervertől!");
                Debug.Log("RESPONSE: " + responseText);
                yield break;
            }

            PlayerPrefs.SetString("token", data.token);
            PlayerPrefs.Save();

            Debug.Log("Sikeres login, token elmentve: " + data.token);


            SceneManager.LoadScene("mainMenu");
        }
    }

    string EscapeJson(string s)
    {
        if (s == null) return "";
        return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
