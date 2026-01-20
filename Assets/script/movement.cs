using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public bool canMove = true;   // EZ A KULCS
    [SerializeField] private Animator Animator;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!canMove) return;   // HA UI FENT VAN → NINCS MOZGÁS

        float move = Input.GetAxisRaw("Horizontal");
        rb.MovePosition(rb.position + Vector2.right * move * speed * Time.fixedDeltaTime);

        if (move != 0)
        {
            Animator.SetBool("isRunning", true);
        }
        else
        {
            Animator.SetBool("isRunning", false);
        }
        if (move < 0) sr.flipX = true;
        else if (move > 0) sr.flipX = false;
    }
}
