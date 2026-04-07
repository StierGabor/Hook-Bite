using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI wormText;
    public TextMeshProUGUI fishText;

    private int fishes;

    void Start()
    {
        Debug.Log("GoldUI Start: Verifying UI references for HUD.");
        if (goldText == null || wormText == null || fishText == null)
            Debug.LogError("GoldUI Start: Missing one or more TextMeshProUGUI references!");
    }

    void Update()
    {
        if (GameManager.Instance == null || goldText == null || wormText == null || fishText == null) 
        {
            Debug.LogWarning("GoldUI: Missing references in Update.");
            return;
        }

        fishes = GameManager.Instance.bream + GameManager.Instance.catfish + GameManager.Instance.ray + GameManager.Instance.octopus;


        goldText.text = "Gold: " + GameManager.Instance.gold;
        wormText.text = "Worms: " + GameManager.Instance.lure;
        fishText.text = "Fishes: " + fishes;
            
    }

}
