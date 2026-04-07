using UnityEngine;

public class FishingUiController : MonoBehaviour
{
    public GameObject fishingPanel;
    private PlayerMovement playerMovement;

    private bool isOpen = false;
    private bool isInFishingZone = false;

    void Start()
    {
        fishingPanel.SetActive(false);

        playerMovement = FindObjectOfType<PlayerMovement>();

        if (playerMovement == null)
            Debug.LogError(" PlayerMovement nem található!");
    }

    void Update()
    {
        if (!isInFishingZone) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!fishingPanel.activeSelf && GameManager.Instance != null && GameManager.Instance.lure <= 0)
            {
                Debug.Log("Nincs elég csalid a horgászathoz!");
                return;
            }

            isOpen = !fishingPanel.activeSelf;
            fishingPanel.SetActive(isOpen);

            // MOZGÁS KI/BE
            playerMovement.canMove = !isOpen;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<FishingZone>() != null)
            isInFishingZone = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<FishingZone>() != null)
        {
            isInFishingZone = false;
            isOpen = false;
            fishingPanel.SetActive(false);
            playerMovement.canMove = true;
        }
    }
    
}
