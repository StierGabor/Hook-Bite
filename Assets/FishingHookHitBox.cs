using UnityEngine;

public class FishingHookHitbox : MonoBehaviour
{
    public CircleCollider2D hookCollider;
    public PlayerFishingProgress fishingProgress;

    // tier 0, 1, 2, 3... sorrendben
    public float[] tierRadii = new float[] { 0.10f, 0.14f, 0.18f, 0.22f };

    void OnEnable()
    {
        ApplyBestTier();
    }

    public void ApplyBestTier()
    {
        if (hookCollider == null || fishingProgress == null) return;

        int tier = Mathf.Clamp(fishingProgress.bestRodTier, 0, tierRadii.Length - 1);
        hookCollider.radius = tierRadii[tier];
    }
}
