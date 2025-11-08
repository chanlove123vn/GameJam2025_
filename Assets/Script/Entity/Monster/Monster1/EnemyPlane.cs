using UnityEngine;

public class EnemyPlane : HuyMonoBehaviour
{
    //===Variables===//
    [Space(25)]
    [Header("===Enemy Plane===")]
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D bodyCol;

    [Header("Status")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private EnemyPlaneState currentState;
    [SerializeField] private Vector2 moveDir;

    [Header("Shotting")]
    [SerializeField] private Bullet bulletPrefab;
    private float shootTimer = 0f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootRate = 1f;



    //===Unity===//
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.firePoint, transform, "FirePoint");
    }

    private void Update()
    {
        this.Moving();
        this.HandleShooting();
    }

    //===Method===//
    private void Moving()
    {
        this.rb.linearVelocity = moveDir * this.moveSpeed;
        Debug.Log(this.transform.position);
    }

    private void HandleShooting()
    {
        if (shootRate <= 0f) return;
        shootTimer += Time.deltaTime;
        if (shootTimer >= 1f / shootRate)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    public void Shoot()
    {
        if (bulletPrefab == null) return;

        // spawn transform: ưu tiên firePoint
        Transform spawnPoint = firePoint != null ? firePoint : this.transform;

        // Lấy pool control cho loại đạn bulletPrefab
        var pool = PoolingManager.Instance.GetPoolCtrl(bulletPrefab);
        if (pool == null) return;

        // Spawn GameObject của viên đạn
        GameObject bulletObj = pool.Spawn(bulletPrefab, spawnPoint.position, spawnPoint.rotation);

        // Lấy component Bullet và thiết lập hướng bay
        Bullet bulletComponent = bulletObj.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            // Bay ngang sang trái (←). Nếu muốn sang phải thì đổi thành Vector2.right
            bulletComponent.Init(Vector2.left);
        }
    }

}
