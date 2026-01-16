using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [Min(0)]
    public int gold = 100; // kezdõ pénz

    public void AddGold(int amount)
    {
        if (amount <= 0) return;
        gold += amount;
        Debug.Log($"Gold: {gold}");
    }

    public bool SpendGold(int amount)
    {
        if (amount <= 0) return true;

        if (gold < amount)
        {
            Debug.Log("Nincs elég gold!");
            return false;
        }

        gold -= amount;
        Debug.Log($"Gold: {gold}");
        return true;
    }
}
