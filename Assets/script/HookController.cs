using UnityEngine;

public class HookController : MonoBehaviour
{
    public RectTransform panelRect;
    public float speed = 200f;

    RectTransform hookRect;

    void Start()
    {
        hookRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(inputX, inputY);

        //  EZ A LÉNYEG
        if (input.magnitude > 1f)
            input = input.normalized;

        Vector2 pos = hookRect.anchoredPosition;
        pos += input * speed * Time.deltaTime;

        // Panel fél méretek
        float panelHalfWidth = panelRect.rect.width / 2f;
        float panelHalfHeight = panelRect.rect.height / 2f;

        // Hook fél méretek
        float hookHalfWidth = hookRect.rect.width / 2f;
        float hookHalfHeight = hookRect.rect.height / 2f;

        // Clamp
        pos.x = Mathf.Clamp(
            pos.x,
            -panelHalfWidth + hookHalfWidth,
             panelHalfWidth - hookHalfWidth
        );

        pos.y = Mathf.Clamp(
            pos.y,
            -panelHalfHeight + hookHalfHeight,
             panelHalfHeight - hookHalfHeight
        );

        hookRect.anchoredPosition = pos;
    }
}
