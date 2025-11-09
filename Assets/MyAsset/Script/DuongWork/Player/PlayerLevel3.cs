using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel3 : PlayerAbstract
{
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private BulletPlayer bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float timer;
    protected override void Update()
    {
        base.Update();
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (!firePoint) firePoint = transform.Find("FirePoint");
    }
    protected override void InitPlayer()
    {
        baseHP = 100;
        HP = baseHP;
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
        timer += Time.deltaTime;
        if (timer >= 0.35f)
        {
            timer = 0;
            var bullet = PoolingManager.Instance.GetPoolCtrl(bulletPrefab).Spawn(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }


}
