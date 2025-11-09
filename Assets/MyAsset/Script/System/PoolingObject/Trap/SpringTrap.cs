using UnityEngine;

// Spring: khi player chạm vào sẽ được bật lên (không gây dmg).
// KHÔNG CẦN SpringPad/SpringBody riêng - chỉ cần 1 GameObject duy nhất!
public class SpringTrap : TrapBase
{
    [Header("Spring")]
    public float bounceForce = 15f;
    public float springHeight = 0.5f;

    [Header("Visual")]
    public float compressionAmount = 0.15f;
    public float compressionSpeed = 10f;

    private Vector3 originalPosition;
    private Vector3 originalScale;
    private float currentCompression = 0f;

    void Awake()
    {
        damage = 0; // không gây sát thương
        originalPosition = transform.position;
        originalScale = transform.localScale;
    }

    void Start()
    {
        LoadComponent();
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();

        // XÓA Rigidbody2D nếu có (không cần!)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            DestroyImmediate(rb);
        }

        // Đảm bảo có Collider2D (KHÔNG trigger)
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider2D>();
        }
        
        col.isTrigger = false; // QUAN TRỌNG!
    }

    void Update()
    {
        // Animate compression - nén cả sprite xuống
        if (currentCompression > 0f)
        {
            currentCompression = Mathf.Max(0f, currentCompression - Time.deltaTime * compressionSpeed);

            // Nén scale Y
            float scaleY = Mathf.Lerp(1f, 0.7f, currentCompression);
            transform.localScale = new Vector3(originalScale.x, originalScale.y * scaleY, originalScale.z);

            // Di chuyển xuống một chút
            transform.position = originalPosition + Vector3.down * (compressionAmount * currentCompression);
        }
    }

    // Override OnCollisionEnter2D để detect trực tiếp
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Reset velocity và bật lên
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

                // Visual feedback
                currentCompression = 1f;
            }
        }
    }

    // Giữ để tương thích TrapBase
    protected override void HandlePlayerHit(GameObject player)
    {
        // Logic đã được xử lý trong OnCollisionEnter2D
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = transform.position;
        Gizmos.DrawWireCube(center, new Vector3(0.5f, springHeight, 0.1f));

        Gizmos.color = Color.yellow;
        float height = bounceForce * 0.2f;
        Gizmos.DrawLine(center, center + Vector3.up * height);
        Gizmos.DrawSphere(center + Vector3.up * height, 0.1f);
    }
}