using UnityEngine;

public class ShopSelectionManager : MonoBehaviour
{
    public static ShopSelectionManager Instance;
    private ItemSlot currentSelected;

    void Awake()
    {
        Instance = this;
    }

    public void Select(ItemSlot slot)
    {
        if (currentSelected != null)
            currentSelected.Deselect();

        currentSelected = slot;
    }
}
