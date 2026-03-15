using UnityEngine;

public static class Auth
{
    public static string Token
    {
        get => PlayerPrefs.GetString("token", "");
        set
        {
            PlayerPrefs.SetString("token", value);
            PlayerPrefs.Save();
        }
    }
}