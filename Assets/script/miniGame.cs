using Unity.VisualScripting;
using UnityEngine;

public class miniGame : MonoBehaviour
{
    public Transform bar;
    public Transform pointer;
    public Collider2D goodZoneCollider;
    public GameObject FishingPanel;
    public PlayerMovement playerMovement; // Reference to player's movement
    public float speed = 5f;
    public float edgeOffset = -0.1f; // Use this to manually fine-tune in the Unity Inspector how far it goes to the edge
    private int direction = 1;
    

    void Update()
    {
        if (bar != null && pointer != null)
        {
            MovePointer();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryCatchFish();
        }
    }

    private void MovePointer()
    {
        float halfWidth = 0f;
        RectTransform rt = bar.GetComponent<RectTransform>();
        if (rt != null)
        {
            // If the bar is a UI element, use its real width
            halfWidth = (rt.rect.width * rt.localScale.x) / 2f;
        }
        else
        {
            // If it is a simple 3D/2D element, based on scale
            halfWidth = bar.localScale.x / 2f;
        }
        
        halfWidth += edgeOffset; // Add the manual correction

        pointer.localPosition += new Vector3(speed * direction * Time.deltaTime, 0, 0);

        if (pointer.localPosition.x >= halfWidth)
        {
            pointer.localPosition = new Vector3(halfWidth, pointer.localPosition.y, pointer.localPosition.z);
            direction = -1;
        }
        else if (pointer.localPosition.x <= -halfWidth)
        {
            pointer.localPosition = new Vector3(-halfWidth, pointer.localPosition.y, pointer.localPosition.z);
            direction = 1;
        }
    }

    private void TryCatchFish()
    {
        if (pointer == null || goodZoneCollider == null) return;

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.lure <= 0)
            {
                Debug.Log("No more lures!");
                return;
            }
            GameManager.Instance.lure--; // Use lure
        }

        Collider2D pointerCollider = pointer.GetComponent<Collider2D>();
        bool caught = false;

        if (pointerCollider != null && pointerCollider.IsTouching(goodZoneCollider))
        {
            caught = true;
        }
        else if (goodZoneCollider.bounds.Contains(pointer.position))
        {
            caught = true;
        }
        
        if (caught)
        {
            Debug.Log("Fish caught!");
            
            int rand = Random.Range(0, 100);
            if (rand < 60)
            {
                GameManager.Instance.bream += 1;
                Debug.Log("Caught a Bream!");
            }
            else if (rand < 85)
            {
                GameManager.Instance.catfish += 1;
                Debug.Log("Caught a Catfish!");
            }
            else if (rand < 98)
            {
                GameManager.Instance.ray += 1;
                Debug.Log("Caught a Ray!");
            }
            else
            {
                GameManager.Instance.octopus += 1;
                Debug.Log("Caught an Octopus!");
            }
        }
        else
        {
            Debug.Log("Missed!");
        }

        if (FishingPanel != null)
        {
            FishingPanel.SetActive(false);
        }

        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }

        if (playerMovement != null)
        {
            playerMovement.canMove = true;
        }
    }
}