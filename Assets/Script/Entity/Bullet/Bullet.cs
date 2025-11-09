using UnityEngine;

public class Bullet : ObjectPooled
{
    //===Variables===//
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 1f;
    private float timer = 0;

    private Rigidbody2D rb;

    //===Unity====//
    private void Update()
    {
        this.DespawningByTime();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if(!rb) this.rb = GetComponent<Rigidbody2D>();
    }
    //===Method===//
    private void Moving()
    {
        float xDir = Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
        float yDir = Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad);

        Vector2 moveDir = new Vector2(xDir, yDir).normalized;
        this.rb.linearVelocity = moveDir * this.speed;
    }

    public override void OnSpawn()
    {
        this.Moving();
        timer = 0;
    }

    private void DespawningByTime()
    {
        timer += Time.deltaTime;
        if (timer >= 2)
        {
            timer = 0;
            PoolingManager.Instance.GetPoolCtrl(this).ReturnToPool(this);
        }
    }

    public void Init(Vector2 direction)
    {
        transform.rotation = Quaternion.Euler(direction);
    }
    
    // Note FUCK U: CHANGE TO PLAYER FUCK UUUUUUUUUU
    private void OnTriggerEnter2D(Collider2D collision) {
    EnemyPlane enemy = collision.GetComponent<EnemyPlane>();
    if (enemy != null) {
        // Debug.Log($"Bullet damage: {damage}");
        // enemy.TakeDamage(damage);
        // PoolingManager.Instance.GetPoolCtrl(this).ReturnToPool(this);
    }
}
}
