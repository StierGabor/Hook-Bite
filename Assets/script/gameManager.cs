
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("API mentéshez")]
    public int userId;
    public int penz;
    public int bot;
    public int halak;
    public int csalik;

    public string bearerToken = "";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveToDatabase()
    {

        //TEST ADATOK KI KELL MAJD SZEDNI
        penz = 100;
        bot = 2;
        halak = 7;
        csalik = 5;

        // 🔥 Itt olvassuk be a login során elmentett adatokat
        userId = PlayerPrefs.GetInt("user_id", -1);
        bearerToken = PlayerPrefs.GetString("token", "");

        Debug.Log("GAME MANAGER USER ID (Unity -> Laravel): " + userId);
        Debug.Log("TOKEN: " + bearerToken);

        StartCoroutine(SaveSystem.UpdateScore(
            userId,
            penz,
            bot,
            halak,
            csalik,
            bearerToken
        ));
    }
}
