using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLevel1 : PlayerAbstract
{

    [SerializeField] private Vector2 moveInput;
    [SerializeField] private BulletPlayerLv1 bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Fire")]
    [SerializeField] private float fireCooldown = 0.35f;
    private float fireTimer;

    

    protected override void Update()
    {
        base.Update();

        if (state == FireMode.Project)
        {
            projectileBuffTimer -= Time.deltaTime;
            if (projectileBuffTimer <= 0f)
            {
                state = FireMode.Normal;
            }
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (!firePoint) firePoint = transform.Find("FirePoint");
    }

    protected override void InitPlayer()
    {
        base.InitPlayer();
        speed = 15;
        dame = 0;
    }

    protected override void Moving()
    {
        if (IsDead || isLockPos) return;
        moveInput = InputManager2.Instance.GetMoveDir();
        rb.linearVelocity = moveInput * speed;
    }

    protected override void Attacking()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer < fireCooldown) return;

        fireTimer = 0f;
        var pool = PoolingManager.Instance.GetPoolCtrl(bulletPrefab);

        if (state == FireMode.Project)
        {
            float[] angles = { -spreadAngle, 0f, spreadAngle };
            foreach (var a in angles)
            {
                var rot = firePoint.rotation * Quaternion.Euler(0f, 0f, a);
                pool.Spawn(bulletPrefab, firePoint.position, rot);
            }
        }
        else
        {
            pool.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    
}
