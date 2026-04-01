using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI wormText;
    public TextMeshProUGUI fishText;

    private int fishes = 0;

    void Update()
    {
        if (GameManager.Instance == null || goldText == null || wormText == null || fishText == null) return;

        fishes = GameManager.Instance.bream + GameManager.Instance.catfish + GameManager.Instance.ray + GameManager.Instance.octopus;

        goldText.text = "Gold: " + GameManager.Instance.gold;
        wormText.text = "Worms: " + GameManager.Instance.lure;
        fishText.text = "Fishes: " + fishes;
            
    }

}
