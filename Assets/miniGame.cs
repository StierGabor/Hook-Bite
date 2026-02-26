using UnityEngine;

public class miniGame : MonoBehaviour
{
    public Transform bar;        // A teljes csík
    public float speed = 5f;     // Mozgás sebessége
    private bool movingRight = true;
    private bool canCatch = false; // true, ha a pointer a jó zónában van

    void Update()
    {
        MovePointer();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canCatch)
            {
                Debug.Log("🎣 Siker! Hal kifogva!");
            }
            else
            {
                Debug.Log("💨 Mellé! A hal elúszott!");
            }
        }
    }

    void MovePointer()
    {
        float halfWidth = bar.localScale.x / 2f;
        Vector3 pos = transform.localPosition;

        pos.x += (movingRight ? 1 : -1) * speed * Time.deltaTime;

        // Ha elérte a széleket, irányt vált
        if (pos.x >= halfWidth) movingRight = false;
        if (pos.x <= -halfWidth) movingRight = true;

        transform.localPosition = pos;
    }

    // Ha a pointer belép a jó zónába
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoodZone"))
            canCatch = true;
    }

    // Ha kilép belőle
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GoodZone"))
            canCatch = false;
    }
}