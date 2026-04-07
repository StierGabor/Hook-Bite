using UnityEngine;

public class BuyItemButton : MonoBehaviour
{
    public int targetrodTier = 1;
    private int itemPrice;

    void Start()
    {
        SetPrice();
        RefreshUI();
    }

    void SetPrice() // Simple pricing logic based on tier
    {
        if (targetrodTier == 1) itemPrice = 100;
        else if (targetrodTier == 2) itemPrice = 200;
        else if (targetrodTier == 3) itemPrice = 500;
    }

    public void RefreshUI()
    {
        if (GameManager.Instance == null) return;

        // Show ONLY the next available upgrade button
        bool shouldShow = GameManager.Instance.rod == targetrodTier - 1;
        gameObject.SetActive(shouldShow);
    }

    public void Buy()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.rod != targetrodTier - 1)
            return;

        // Attempt to spend gold and upgrade the rod - Main Purchase Logic
        if (GameManager.Instance.Spendgold(itemPrice))
        {
            GameManager.Instance.rod = targetrodTier;
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