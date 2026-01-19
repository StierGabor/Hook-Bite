using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool canMove = true;   // EZ A KULCS

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!canMove) return;   // HA UI FENT VAN → NINCS MOZGÁS

        float move = Input.GetAxisRaw("Horizontal");
        rb.MovePosition(rb.position + Vector2.right * move * speed * Time.fixedDeltaTime);
    }
}
