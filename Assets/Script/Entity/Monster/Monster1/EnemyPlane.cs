using UnityEngine;

public class EnemyPlane : MonsterAbstract
{
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

        Bullet bullet = PoolingManager.Instance.GetPoolCtrl(bulletPrefab)
                                                .Spawn(bulletPrefab,firePoint.position, firePoint.rotation)
                                                .GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.Init(bulletDirection);
        }
    }
}
