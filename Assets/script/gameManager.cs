using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("API mentéshez")]

    /// <summary>
    /// ////////////////////////    
    /// 
    /// A játékos adatait itt tároljuk, amiket majd a SaveSystem segítségével elküldünk a szervernek.
    /// Ugyan akkor ezek itt nem csak a szerver adatai hanem a runtime változók is, amiket a játék során használunk.
    /// Jelenleg a pénz van össze kötve a GOLD UI-al és a miniGame-el is, a többi változó még nincs használva.
    /// 
    /// </summary>
    /// 

    public int userId;
    public int penz;
    public int bot;
    public int halak;
    public int csalik;

    /// <summary>
    /// ////////////////////////    
    /// </summary>
    /// 

    public string bearerToken = "";

    public void Start()
    {
        //játék elején betöltjük a játékos alap felszerelését
        penz = 0;
        bot = 0;
        halak = 0;
        csalik = 0;
    }

    //gold management
    public void AddPenz(int amount)
    {
        if (amount <= 0) return;
        penz += amount;
        Debug.Log($"Gold: {penz}");
    }

    public bool SpendPenz(int amount)
    {
        if (amount <= 0) return true;

        if (penz < amount)
        {
            Debug.Log("Nincs elég penz!");
            return false;
        }

        penz -= amount;
        Debug.Log($"penz: {penz}");
        return true;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ← THIS is key
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveToDatabase()
    {
            Debug.Log("Saving data to database...");

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

            Debug.Log("User ID: " + userId);
            Debug.Log("Pénz: " + penz);
            Debug.Log("Bot: " + bot);
            Debug.Log("Halak: " + halak);
            Debug.Log("Csalik: " + csalik);
    }
}