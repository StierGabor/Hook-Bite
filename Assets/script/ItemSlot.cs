using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public Image highlight;

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopSelectionManager.Instance.Select(this);

        if (highlight != null)
            highlight.enabled = true;
    }

    public void Deselect()
    {
        if (highlight != null)
            highlight.enabled = false;
    }
}
