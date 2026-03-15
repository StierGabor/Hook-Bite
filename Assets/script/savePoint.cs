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
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            promptUI?.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptUI?.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Mentés folyamatban...");
            GameManager.Instance.SaveToDatabase();
        }
    }
        
}