using UnityEngine;

public class BuyItemButton : MonoBehaviour
{
    public GameManager gameManager; //a játékos pénzét innen vesszük
    public PlayerFishingProgress fishingProgress;

    public int firstItemPrice = 10;
    public int secondItemPrice = 50;
    public int thirdItemPrice = 500;
    bool success = false;

    public void Buy()
    {
        if (gameManager == null) return;

        if (gameManager.bot == 0)
        {
                success = gameManager.SpendPenz(firstItemPrice);
            gameManager.bot = 1;
        }
        else if (gameManager.bot == 1)
        {
                success = gameManager.SpendPenz(secondItemPrice);
            gameManager.bot = 2;
        }
        else if (gameManager.bot == 2)
        {
                success = gameManager.SpendPenz(thirdItemPrice);
            gameManager.bot = 3;
        }

        if (success)
        {
            if (gameManager.bot > 0 && fishingProgress != null)
                fishingProgress.UnlockRodTier(gameManager.bot);

            gameObject.SetActive(false);
        }

        else
        {
            Debug.Log("Nincs elég gold");
        }
    }
}
