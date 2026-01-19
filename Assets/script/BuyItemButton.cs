using UnityEngine;

public class BuyItemButton : MonoBehaviour
{
    public int itemPrice = 10;
    public PlayerGold playerGold;
    public int rodTier = 0; // 0 = nem bot, 1..N = bot tier
    public PlayerFishingProgress fishingProgress;


    public void Buy()
    {
        if (playerGold == null) return;

        bool success = playerGold.SpendGold(itemPrice);

        if (success)
        {
            if (rodTier > 0 && fishingProgress != null)
                fishingProgress.UnlockRodTier(rodTier);

            gameObject.SetActive(false);
        }

        else
        {
            Debug.Log("Nincs elég gold");
        }
    }
}
