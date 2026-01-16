using UnityEngine;

public class PlayerGold_Old : MonoBehaviour
{
    public int gold = 100; // kezdõ pénz

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold növelve. Jelenlegi gold: " + gold);
    }

    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log("Gold elköltve. Jelenlegi gold: " + gold);
            return true;
        }

        Debug.Log("Nincs elég gold!");
        return false;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G)) AddGold(10);
        if (Input.GetKeyDown(KeyCode.H)) SpendGold(30);
    }


}
