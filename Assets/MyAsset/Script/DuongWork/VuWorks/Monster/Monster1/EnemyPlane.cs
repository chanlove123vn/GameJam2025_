using UnityEngine;

public class EnemyPlane : MonsterAbstract
{
    //===Variables===//
    [Header("===Shotting===")]
    [SerializeField] private Vector2 bulletDirection = Vector2.down;
    [SerializeField] protected BulletEnemy bulletPrefab;
    [SerializeField] protected Transform firePoint;
    // private float spawnTime = 0f; // Timer để despawn sau lmao s

    //===Unity===//
    protected override void LoadComponent()
    {
        if (!firePoint) this.firePoint = this.transform.Find("FirePoint");
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        // spawnTime = Time.time; // Ghi lại thời gian spawn
    }

    //===Method===//
    protected override void Moving()
    {
        currentState = MonsterState.MOVING;
        this.rb.linearVelocity = moveDir * this.moveSpeed;
        Debug.Log(this.transform.position);
    }

    private void Update()
    {
        this.Moving();
        this.Shooting();
    }

    public void Shooting()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            this.Shoot();
        }
    }

    public void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        BulletEnemy bullet = PoolingManager.Instance.GetPoolCtrl(bulletPrefab)
                                                .Spawn(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, -90))
                                                .GetComponent<BulletEnemy>();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.Init(bulletDirection);
        }
    }
}
