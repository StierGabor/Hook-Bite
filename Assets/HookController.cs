using UnityEngine;

public class HookController : MonoBehaviour
{
    public RectTransform panelRect; // a UI panel
    public float speed = 300f;      // alap sebesség

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (panelRect == null)
            Debug.LogError("A panelRect nincs hozzárendelve a HookController-ben!");
    }

    void Update()
    {
        if (panelRect == null) return;

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        // a sebesség most már arányos a panel teljes méretével
        float speedX = speed * Time.deltaTime;
        float speedY = speed * Time.deltaTime;

        Vector2 newPos = rectTransform.anchoredPosition;
        newPos.x += inputX * speedX;
        newPos.y += inputY * speedY;

        // panel határain belülre korlátozás
        float halfWidth = panelRect.rect.width / 2f;
        float halfHeight = panelRect.rect.height / 2f;

        newPos.x = Mathf.Clamp(newPos.x, -halfWidth, halfWidth);
        newPos.y = Mathf.Clamp(newPos.y, -halfHeight, halfHeight);

        rectTransform.anchoredPosition = newPos;
    }
}
