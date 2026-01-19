using UnityEngine;

public class PlayerFishingProgress : MonoBehaviour
{
    [Min(0)]
    public int bestRodTier = 0; // 0 = alap, 1..N = jobb botok

    public void UnlockRodTier(int tier)
    {
        if (tier > bestRodTier)
            bestRodTier = tier;
    }
}
