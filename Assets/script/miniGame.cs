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
    
    public movement fishMovement;

    private Vector2 initialBoxSize;
    private float initialCircleRadius;
    private int colliderType = 0; // 0 = None, 1 = Box, 2 = Circle
    private bool isColliderInitialized = false;

    void OnEnable()
    {
        if (!isColliderInitialized && goodZoneCollider != null)
        {
            if (goodZoneCollider is BoxCollider2D box)
            {
                initialBoxSize = box.size;
                colliderType = 1;
                isColliderInitialized = true;
            }
            else if (goodZoneCollider is CircleCollider2D circle)
            {
                initialCircleRadius = circle.radius;
                colliderType = 2;
                isColliderInitialized = true;
            }
        }
        ApplyRodMultiplier();
    }

    private void ApplyRodMultiplier()
    {
        if (goodZoneCollider == null || GameManager.Instance == null || !isColliderInitialized) return;
        
        float multiplier = 1f;
        int rodLevel = GameManager.Instance.rod; // Assuming rod level is stored in GameManager
        if (rodLevel == 0) multiplier = 0.8f;
        else if (rodLevel == 1) multiplier = 1.2f;
        else if (rodLevel == 2) multiplier = 2f;
        else if (rodLevel == 3) multiplier = 5f;

        if (colliderType == 1)
        {
            ((BoxCollider2D)goodZoneCollider).size = initialBoxSize * multiplier;
        }
        else if (colliderType == 2)
        {
            ((CircleCollider2D)goodZoneCollider).radius = initialCircleRadius * multiplier;
        }
    }

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
            
            // Try to find the movement script if not explicitly assigned
            if (fishMovement == null && goodZoneCollider != null)
            {
                fishMovement = goodZoneCollider.GetComponent<movement>();
            }

            

            int caughtIndex = 0; // Default to Bream
            if (fishMovement != null)
            {
                caughtIndex = fishMovement.currentFishIndex;
            }
            
            if (caughtIndex == 0)
            {
                GameManager.Instance.bream += 1;
                Debug.Log("Caught a Bream!");
            }
            else if (caughtIndex == 1)
            {
                GameManager.Instance.catfish += 1;
                Debug.Log("Caught a Catfish!");
            }
            else if (caughtIndex == 2)
            {
                GameManager.Instance.ray += 1;
                Debug.Log("Caught a Ray!");
            }
            else if (caughtIndex == 3)
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