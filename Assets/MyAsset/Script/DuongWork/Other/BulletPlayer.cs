using UnityEngine;


public class BulletPlayer : ObjectPooled
{
    [SerializeField] private float speed = 20;
    [SerializeField] private float lifeTime = 10;
    [SerializeField] private float timer = 0;
    [SerializeField] private Collider2D hitPoint;
    private void Update()
    {
        Flying();
        BulletDisspearAfterLife();
    }
    protected override void LoadComponent()
    {
        base.LoadComponent();
        hitPoint = GetComponent<Collider2D>();
    }
    private void Flying()
    {
        var direction = transform.up;
        transform.position += direction.normalized * speed * Time.deltaTime;

    }
    public override void OnSpawn()
    {
        timer = 0f;
    }
    public override void OnDespawn()
    {
        base.OnDespawn();
    }
    private void TurnOffBullet()
    {
        PoolingManager.Instance.GetPoolCtrl(this).ReturnToPool(this);
    }
    private void BulletDisspearAfterLife()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            timer = 0f;
            TurnOffBullet();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if(collider.GetComponent<Enemy>)
        
    }

}