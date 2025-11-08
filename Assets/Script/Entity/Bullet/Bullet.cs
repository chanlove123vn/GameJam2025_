using UnityEngine;

public class Bullet : ObjectPooled
{
    //--Variables--//
    [SerializeField] private float speed = 10f;
    [SerializeField] private float bulletLife = 5f;
    [SerializeField] private float damage = 1f;

    // cache Rigidbody2D if present
    private Rigidbody2D rb;

    public override void OnSpawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        // Mỗi lần spawn lại thì reset vận tốc
        if (rb) rb.linearVelocity = Vector2.zero;

        // Bắt đầu đếm thời gian sống của đạn
        CancelInvoke(nameof(Despawn));
        Invoke(nameof(Despawn), bulletLife);
    }

    public override void OnDespawn()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (rb) rb.linearVelocity = Vector2.zero;

        CancelInvoke(nameof(Despawn));
    }

    // Hàm trả đạn về pool sau khi hết bulletLife
    private void Despawn()
    {
        var pool = PoolingManager.Instance.GetPoolCtrl(this);
        if (pool != null)
        {
            pool.ReturnToPool(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// /// Khởi tạo hướng bay, speed & bulletLife dùng giá trị nội bộ của Bullet
    public void Init(Vector2 direction)
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        if (direction != Vector2.zero)
        {
            direction.Normalize();
            // Cho "đầu đạn" quay theo hướng bay
            transform.up = direction;
        }

        if (rb)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    // Nếu sau này cần lấy damage từ chỗ khác thì dùng property này
    public float Damage => damage;
}
