using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject shopPanel;
    public PlayerMovement playerMovement;
    private bool isOpen = false;
    private bool isInShopZone = false;

    void Update()
    {
        if (isInShopZone && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            shopPanel.SetActive(isOpen);
            playerMovement.canMove = !isOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShopZone"))
        {
            isInShopZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ShopZone"))
        {
            isInShopZone = false;
            shopPanel.SetActive(false);
            isOpen = false;
        }
    }
}
