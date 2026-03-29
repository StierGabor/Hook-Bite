using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject wormPanel;
    public PlayerMovement playerMovement;
    private bool isOpen = false;
    private bool isInShopZone = false;
    private bool isInWormZone = false;

    void Update()
    {
        if (isInShopZone && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            shopPanel.SetActive(isOpen);
            playerMovement.canMove = !isOpen;
        }

        else if (isInWormZone && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
            wormPanel.SetActive(isOpen);
            playerMovement.canMove = !isOpen;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ShopZone"))
        {
            isInShopZone = true;
        }

        else if (other.CompareTag("WormZone"))
        {
            isInWormZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("ShopZone") && !other.CompareTag("WormZone"))
            return;

        isInShopZone = false;
        isInWormZone = false;
        isOpen = false;

        if (shopPanel != null)
            shopPanel.SetActive(false);

        if (wormPanel != null)
            wormPanel.SetActive(false);

        if (playerMovement != null)
            playerMovement.canMove = true;
    }
}
