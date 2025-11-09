using UnityEngine;


public class BulletPlayerLv1 : ObjectPooled
{
    [SerializeField] protected float speed = 20;
    [SerializeField] protected float lifeTime = 10;
    [SerializeField] protected float timer = 0;
    [SerializeField] protected int damage = 1;
    [SerializeField] protected Collider2D hitPoint;
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
    protected virtual void Flying()
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
    protected void TurnOffBullet()
    {
        PoolingManager.Instance.GetPoolCtrl(this).ReturnToPool(this);
    }
    protected void BulletDisspearAfterLife()
    {
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            timer = 0f;
            TurnOffBullet();
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent<MonsterAbstract>(out var monster))
        {
            monster.TakeDamage(damage);
            TurnOffBullet();
        }
        
    }

}