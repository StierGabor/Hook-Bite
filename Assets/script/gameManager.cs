using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("For API saving")]

    /// <summary>
    /// ////////////////////////    
    /// 
    /// Player data is stored here, which will be sent to the server using SaveSystem.
    /// At the same time, these are not only server data but also runtime variables used during the game.
    /// Currently, gold is connected to the GOLD UI and the miniGame, other variables are not used yet.
    /// 
    /// </summary>
    /// 

    public int userId; //Player ID, Used to identify the player in the database. This will be set when the player logs in.
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
        Debug.Log("GameManager Start: Initializing base gear to 0.");
        // Load the player's basic equipment at the start of the game
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
        if (amount <= 0) 
        {
            Debug.LogWarning($"GameManager Addgold: Attempted to add invalid amount {amount}");
            return;
        }
        gold += amount;
        Debug.Log($"GameManager Addgold: Gold increased by {amount}. New total: {gold}");
    }

    public bool Spendgold(int amount)
    {
        if (amount <= 0) 
        {
            Debug.LogWarning($"GameManager Spendgold: Attempted to spend invalid amount {amount}");
            return true;
        }

        if (gold < amount)
        {
            Debug.LogWarning($"GameManager Spendgold: Not enough gold! Required: {amount}, Current: {gold}");
            return false;
        }

        gold -= amount;
        Debug.Log($"GameManager Spendgold: Spent {amount}. Remaining gold: {gold}");
        return true;
    }

    private void Awake()
    {
        Debug.Log("GameManager Awake: Checking instance.");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ← THIS is key
            Debug.Log("GameManager Awake: Instance created and DontDestroyOnLoad set.");
        }
        else
        {
            Debug.Log("GameManager Awake: Duplicate instance destroyed.");
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
            Debug.Log("Gold: " + gold);
            Debug.Log("rod: " + rod);
            Debug.Log("bream: " + bream);
            Debug.Log("lure: " + lure);
    }
}