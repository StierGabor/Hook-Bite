using UnityEngine;

public class BuyItemButton : MonoBehaviour
{
    public int targetBotTier = 1;
    private int itemPrice;

    void Start()
    {
        SetPrice();
        RefreshUI();
    }

    void SetPrice()
    {
        if (targetBotTier == 1) itemPrice = 100;
        else if (targetBotTier == 2) itemPrice = 200;
        else if (targetBotTier == 3) itemPrice = 500;
    }

    public void RefreshUI()
    {
        if (GameManager.Instance == null) return;

        // Show ONLY the next available upgrade
        bool shouldShow = GameManager.Instance.bot == targetBotTier - 1;
        gameObject.SetActive(shouldShow);
    }

    public void Buy()
    {
        if (GameManager.Instance == null) return;

        // Check again (safety)
        if (GameManager.Instance.bot != targetBotTier - 1)
            return;

        if (GameManager.Instance.SpendPenz(itemPrice))
        {
            GameManager.Instance.bot = targetBotTier;

            // 🔥 Refresh ALL buttons after purchase
            RefreshAllButtons();
        }
        else
        {
            Debug.Log("Nincs elég gold");
        }
    }

    void RefreshAllButtons()
    {
        BuyItemButton[] buttons = FindObjectsOfType<BuyItemButton>(true);

        foreach (var btn in buttons)
        {
            btn.RefreshUI();
        }
    }
}