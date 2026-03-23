using UnityEngine;

public class BuyItemButton : MonoBehaviour
{
    public PlayerFishingProgress fishingProgress;

    public int targetBotTier = 1; // Állítsd be az Inspectorban (1 az első bothoz, 2 a másodikhoz, 3 a harmadikhoz)

    public int firstItemPrice = 10;
    public int secondItemPrice = 50;
    public int thirdItemPrice = 500;

    void Update()
    {
        if (GameManager.Instance == null) return;

        // Csak azt a gombot mutatjuk, amelyik a következő megvásárolható bot
        if (GameManager.Instance.bot >= targetBotTier)
        {
            // Már megvásárolta ezt a botot
            gameObject.SetActive(false);
        }
        else if (GameManager.Instance.bot == targetBotTier - 1)
        {
            // Ez a következő bot, amit megvehet
        }
        else
        {
            // Még nem vette meg az előző botokat
            gameObject.SetActive(false);
        }
    }

    public void Buy()
    {
        if (GameManager.Instance == null) return;

        bool success = false;

        // Cél bot szint szerint ár meghatározás és vásárlás
        if (targetBotTier == 1 && GameManager.Instance.bot == 0)
        {
            success = GameManager.Instance.SpendPenz(firstItemPrice);
            if (success) GameManager.Instance.bot = 1;
        }
        else if (targetBotTier == 2 && GameManager.Instance.bot == 1)
        {
            success = GameManager.Instance.SpendPenz(secondItemPrice);
            if (success) GameManager.Instance.bot = 2;
        }
        else if (targetBotTier == 3 && GameManager.Instance.bot == 2)
        {
            success = GameManager.Instance.SpendPenz(thirdItemPrice);
            if (success) GameManager.Instance.bot = 3;
        }

        if (success)
        {
            if (GameManager.Instance.bot > 0 && fishingProgress != null)
                fishingProgress.UnlockRodTier(GameManager.Instance.bot);

            // Ha a gomb egy adott bothoz tartozik, elrejthető
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Nincs elég gold");
        }
    }
}