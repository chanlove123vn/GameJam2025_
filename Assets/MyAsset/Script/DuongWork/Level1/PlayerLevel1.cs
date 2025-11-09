using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerLevel1 : PlayerAbstract
{

    [SerializeField] private Vector2 moveInput;
    [SerializeField] private BulletPlayerLv1 bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Fire")]
    [SerializeField] private float fireCooldown = 0.5f;
    [SerializeField] private float delayBullet = 0.03f;
    [SerializeField] private bool isBursting = false; 
    private float fireTimer;



    protected override void Update()
    {
        base.Update();

        if (state == FireMode.ProjectAngle)
        {
            projectileBuffTimer -= Time.deltaTime;
            if (projectileBuffTimer <= 0f)
            {
                state = FireMode.Normal;
            }
        }
        if (state == FireMode.ProjectStraight)
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
        fireCooldown = .5f;
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

        if (state == FireMode.ProjectAngle)
        {
            float[] angles = { -spreadAngle, 0f, spreadAngle };
            foreach (var a in angles)
            {
                var rot = firePoint.rotation * Quaternion.Euler(0f, 0f, a);
                pool.Spawn(bulletPrefab, firePoint.position, rot);
            }
        }
        else if (state == FireMode.ProjectStraight)
        {
            if (!isBursting) StartCoroutine(FireStraightBurst());

        }
        else if (state == FireMode.Normal)
        {
            pool.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private IEnumerator FireStraightBurst()
    {
        isBursting = true;
        var pool = PoolingManager.Instance.GetPoolCtrl(bulletPrefab);

        for (int i = 0; i < 3; i++)
        {
            pool.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);
            if (i < 2) yield return new WaitForSeconds(delayBullet);
        }

        isBursting = false;
    }
}
