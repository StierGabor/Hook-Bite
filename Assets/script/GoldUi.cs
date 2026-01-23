using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public PlayerGold playerGold;
    public TextMeshProUGUI goldText;

    void Update()
    {
        if (playerGold == null || goldText == null) return;

        goldText.text = "Gold: " + playerGold.gold;
    }
}
