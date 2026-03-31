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
    public int gold;
    public int rod;
    public int bream;
    public int catfish;
    public int ray;
    public int octopus;
    public int lure;

    /// <summary>
    /// ////////////////////////    
    /// </summary>
    /// 

    public string bearerToken = "";

    public void Start()
    {
        //játék elején betöltjük a játékos alap felszerelését
        gold = 0;
        rod = 0;
        bream = 0;
        catfish = 0;
        ray = 0;
        octopus = 0;
        lure = 0;
    }

    //gold management
    public void Addgold(int amount)
    {
        if (amount <= 0) return;
        gold += amount;
        Debug.Log($"Gold: {gold}");
    }

    public bool Spendgold(int amount)
    {
        if (amount <= 0) return true;

        if (gold < amount)
        {
            Debug.Log("Nincs elég gold!");
            return false;
        }

        gold -= amount;
        Debug.Log($"gold: {gold}");
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
                gold,
                rod,
                bream,
                catfish,
                ray,
                octopus,
                lure,
                bearerToken
            ));

            Debug.Log("User ID: " + userId);
            Debug.Log("Pénz: " + gold);
            Debug.Log("rod: " + rod);
            Debug.Log("bream: " + bream);
            Debug.Log("lure: " + lure);
    }
}