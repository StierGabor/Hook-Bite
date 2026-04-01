using UnityEngine;

public class SavePoint : MonoBehaviour
{
    public GameObject promptUI;
    private bool playerInRange = false;

    private void Start()
    {
        promptUI?.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Prevent accidental save triggering if this script was copy-pasted to other interaction zones
        if (gameObject.tag == "ShopZone" || gameObject.tag == "WormZone" || gameObject.name.Contains("Fish") || gameObject.tag == "SaveZone")
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;
                promptUI?.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Prevent accidental save triggering if this script was copy-pasted to other interaction zones
        if (gameObject.tag == "ShopZone" || gameObject.tag == "WormZone" || gameObject.name.Contains("Fish") || gameObject.tag == "SaveZone")
        {

            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                promptUI?.SetActive(false);
            }
        }
     }

    private void Update()
    {
        if (playerInRange && gameObject.tag == "SaveZone" && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Mentés folyamatban...");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveToDatabase();
            }
            else
            {
                Debug.LogWarning("Nem található GameManager a jelenetben a mentéshez!");
            }
        }
    }
        
}