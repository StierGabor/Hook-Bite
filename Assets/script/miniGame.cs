using UnityEngine;

public class miniGame : MonoBehaviour
{
    public Transform bar;
    public Transform pointer;
    public Collider2D goodZoneCollider;
    public GameObject FishingPanel;
    public PlayerMovement playerMovement; // Referencia a játékos mozgására

    public float speed = 5f;
    public float edgeOffset = -0.1f; // Ezzel a Unity Inspectorban manuálisan finomhangolhatod, mennyire menjen ki a széléig
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
            // Ha a bar egy UI elem, a valós szélességét használjuk
            halfWidth = (rt.rect.width * rt.localScale.x) / 2f;
        }
        else
        {
            // Ha sima 3D/2D elem, akkor a scale alapján
            halfWidth = bar.localScale.x / 2f;
        }
        
        halfWidth += edgeOffset; // Hozzáadjuk a manuális korrekciót

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