using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public GameManager gameManager;

    void Update()
    {
        if (gameManager == null || goldText == null) return;

        goldText.text = "Gold: " + gameManager.penz;
    }
}
