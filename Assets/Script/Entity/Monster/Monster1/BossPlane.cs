using UnityEngine;

public class BossPlane : MonsterAbstract
{
    [Header("Boss Specific")]
    [SerializeField] protected float shootInterval = 0.5f;
    private float shootTimer = 0f;
    private float spawnTime = 0f;
    
    [SerializeField] private Vector2 bulletDirection = Vector2.down;
    [SerializeField] protected BulletEnemy bulletPrefab;
    [SerializeField] protected Transform firePoint;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        
        if (!firePoint)
        {
            firePoint = this.transform.Find("FirePoint");
        }
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        spawnTime = Time.time;
        shootTimer = 0f;
    }

    protected override void Moving()
    {
        currentState = MonsterState.MOVING;
        this.rb.linearVelocity = moveDir * this.moveSpeed;
    }

    protected virtual void Update()
    {
        this.Moving();
        this.Shooting();
        
        // ⭐ Despawn sau 20s (Boss sống lâu hơn)
        if (Time.time - spawnTime > 20f)
        {
            PoolingManager.Instance.GetPoolCtrl(this).ReturnToPool(this);
        }
    }

    public void Shooting()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            this.Shoot();
        }
    }

    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        BulletEnemy bullet = PoolingManager.Instance.GetPoolCtrl(bulletPrefab)
            .Spawn(bulletPrefab, firePoint.position, Quaternion.identity)
            .GetComponent<BulletEnemy>();

        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.Init(bulletDirection);
        }
    }
}
