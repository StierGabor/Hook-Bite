using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    void Update()
    {
        if (GameManager.Instance == null || goldText == null) return;

        goldText.text = "Gold: " + GameManager.Instance.penz;
    }
}
