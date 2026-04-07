using UnityEngine;

public static class Auth
{
    public static string Token
    {
        get 
        {
            string token = PlayerPrefs.GetString("token", "");
            Debug.Log($"Auth Token Get: {(string.IsNullOrEmpty(token) ? "No token found" : "Token retrieved")}");
            return token;
        }
        set
        {
            Debug.Log("Auth Token Set: Setting new token and saving.");
            PlayerPrefs.SetString("token", value);
            PlayerPrefs.Save();
        }
    }
}