using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FishAI_Fishy : MonoBehaviour
{
    public RectTransform panelRect;
    public float moveSpeed = 30f;
    public float rotationSpeed = 90f;
    public float minDirectionTime = 0.8f;
    public float maxDirectionTime = 2.5f;
    public float minPauseTime = 0.3f;
    public float maxPauseTime = 1.2f;
    public float pauseChance = 0.15f;
    public float speedVariation = 0.3f;

    private RectTransform rectTransform;
    private Vector2 direction;
    private Vector2 velocity;
    private float currentSpeed;
    private float targetSpeed;
    private float directionTimer;
    private float pauseTimer;
    private bool isPaused;
    private Vector3 originalScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
        
        direction = Random.insideUnitCircle.normalized;
        currentSpeed = moveSpeed;
        targetSpeed = moveSpeed;
        directionTimer = Random.Range(minDirectionTime, maxDirectionTime);
        
        rectTransform.anchoredPosition = new Vector2(
            Random.Range(-panelRect.rect.width * 0.4f, panelRect.rect.width * 0.4f),
            Random.Range(-panelRect.rect.height * 0.4f, panelRect.rect.height * 0.4f)
        );
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (isPaused)
        {
            pauseTimer -= dt;
            if (pauseTimer <= 0)
            {
                isPaused = false;
                PickNewDirection();
            }
            return;
        }

        directionTimer -= dt;
        if (directionTimer <= 0)
        {
            if (Random.value < pauseChance)
            {
                isPaused = true;
                pauseTimer = Random.Range(minPauseTime, maxPauseTime);
                currentSpeed = 0;
                targetSpeed = 0;
            }
            else
            {
                PickNewDirection();
            }
        }

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, dt * 2f);
        
        if (direction.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float currentAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * dt);
            velocity = new Vector2(Mathf.Cos(newAngle * Mathf.Deg2Rad), Mathf.Sin(newAngle * Mathf.Deg2Rad));
        }

        Vector2 newPos = rectTransform.anchoredPosition + velocity * currentSpeed * dt;

        float halfWidth = panelRect.rect.width * 0.5f;
        float halfHeight = panelRect.rect.height * 0.5f;
        float fishWidth = rectTransform.rect.width * 0.5f * Mathf.Abs(rectTransform.lossyScale.x);
        float fishHeight = rectTransform.rect.height * 0.5f * Mathf.Abs(rectTransform.lossyScale.y);

        bool bounced = false;

        if (newPos.x - fishWidth < -halfWidth || newPos.x + fishWidth > halfWidth)
        {
            direction.x = -direction.x;
            velocity.x = -velocity.x;
            newPos.x = Mathf.Clamp(newPos.x, -halfWidth + fishWidth, halfWidth - fishWidth);
            bounced = true;
        }

        if (newPos.y - fishHeight < -halfHeight || newPos.y + fishHeight > halfHeight)
        {
            direction.y = -direction.y;
            velocity.y = -velocity.y;
            newPos.y = Mathf.Clamp(newPos.y, -halfHeight + fishHeight, halfHeight - fishHeight);
            bounced = true;
        }

        if (bounced)
        {
            directionTimer = Random.Range(minDirectionTime * 0.5f, maxDirectionTime * 0.5f);
        }

        rectTransform.anchoredPosition = newPos;

        if (velocity.x != 0)
        {
            Vector3 scale = originalScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(velocity.x);
            rectTransform.localScale = scale;
        }
    }

    void PickNewDirection()
    {
        float angle = Random.Range(-60f, 60f) * Mathf.Deg2Rad;
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        direction = new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos
        ).normalized;

        targetSpeed = moveSpeed * Random.Range(1f - speedVariation, 1f + speedVariation);
        directionTimer = Random.Range(minDirectionTime, maxDirectionTime);
    }
}
