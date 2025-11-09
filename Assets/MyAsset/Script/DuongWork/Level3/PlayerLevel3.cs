using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel3 : PlayerAbstract
{
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float speedUp;
    [SerializeField] private float speedDown;
    [SerializeField] private float timer;

    [SerializeField] private float coyoteTime = 0.10f;
    [SerializeField] private float jumpBuffer = 0.10f;
    [SerializeField, Range(0f, 1f)] private float jumpCut = 0.5f;

    private float coyoteTimer;
    private float bufferTimer;

    //Jumping
    [SerializeField] private float jumpPower;
    public LayerMask groundLayer;
    [SerializeField] private float groundCheckOffset = 0.02f;
    public float groundCheckRadius = 0.05f;
    private Collider2D[] groundHits = new Collider2D[4];
    [SerializeField] private float baseGravity = 3f;
    [SerializeField] private float fallGravityMult = 2.2f;
    [SerializeField] private float jumpCutMult = 2.8f;
    [SerializeField] private float apexHangMult = 0.6f;
    [SerializeField] private float apexThreshold = 0.15f;
    [SerializeField] private float maxFallSpeed = 18;

    [SerializeField] private float apexBonusSpeed = 2f;
    [SerializeField] private Collider2D groundBox;
    private ContactFilter2D _groundFilter;
    private float apexPoint;
    private float apexBonus;

    private bool jumpHeld;

    //Shooting
    [Header("Fire")]
    [SerializeField] private float fireCommonCooldown;
    [SerializeField] private float fireSpecialHoldingTime;
    [SerializeField] private float fireTimer;
    [SerializeField] private float fireHoldingTimer;
    private Transform firePoint;
    [SerializeField] private BulletPlayerLv1 bulletCommonPrefab;
    [SerializeField] private BulletPlayerLv1 bulletSpecialPrefab;

    //Buff Projectile
    // PlayerLevel3
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstInterval = 0.08f;
    private int burstLeft;
    private float burstTimer;



    protected override void Update()
    {
        base.Update();

        MoveTimerCount();

        Jumping();
        if (state == FireMode.ProjectStraight)
        {
            projectileBuffTimer -= Time.deltaTime;
            if (projectileBuffTimer <= 0f) state = FireMode.Normal;
        }
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (!groundBox) groundBox = transform.Find("GroundBox").GetComponent<Collider2D>();
        _groundFilter.useLayerMask = true;
        _groundFilter.layerMask = groundLayer;
        _groundFilter.useTriggers = true;
        if (!firePoint) firePoint = transform.Find("FirePoint");
    }
    protected override void InitPlayer()
    {
        base.InitPlayer();
        speed = 25;
        dame = 0;
        speedUp = 1f;
        jumpPower = 18f;
        speedDown = 1.5f;
        fireTimer = 0;
        fireCommonCooldown = 0.5f;
        fireSpecialHoldingTime = 5f;
        if (rb != null) rb.gravityScale = baseGravity;
        fireTimer = fireCommonCooldown;
    }

    protected override void Moving()
    {
        if (IsDead || isLockPos) return;

        moveInput = InputManager4.Instance.GetMoveDir();
        if (Mathf.Abs(moveInput.x) > 0.01f)
            facing = moveInput.x > 0 ? 1 : -1;

        if (sprite) sprite.flipX = (facing == -1);


        float maxSpeed = speed + apexBonus;

        float targetVx = moveInput.x * maxSpeed;
        float currentVx = rb.linearVelocity.x;

        bool hasInput = Mathf.Abs(moveInput.x) > 0.01f;
        bool sameDir = Mathf.Sign(targetVx) == Mathf.Sign(currentVx) || Mathf.Abs(currentVx) < 0.01f;

        float rate = hasInput ? (sameDir ? speedUp : speedDown) : speedDown;

        float speedDiff = targetVx - currentVx;
        float forceX = speedDiff * rate;
        rb.AddForce(new Vector2(forceX, 0f), ForceMode2D.Force);

        float newVx = rb.linearVelocity.x;
        if (Mathf.Abs(newVx) > maxSpeed)
            rb.linearVelocity = new Vector2(Mathf.Sign(newVx) * maxSpeed, rb.linearVelocity.y);
    }

    private void Jumping()
    {
        if (IsDead || isLockPos) return;
        bool jumpPressed = false;
        var im = InputManager4.Instance;
        if (im != null)
        {
            jumpPressed = im.GetJump();
            jumpHeld = im.GetJumpHeld();
        }

        if (jumpPressed) bufferTimer = jumpBuffer;

        if (IsGrounded()) coyoteTimer = coyoteTime;

        if (bufferTimer > 0f && coyoteTimer > 0f)
        {
            bufferTimer = 0f;
            coyoteTimer = 0f;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        float vy = rb.linearVelocity.y;
        apexPoint = Mathf.InverseLerp(apexThreshold, 0f, Mathf.Abs(vy));
        apexBonus = Mathf.Lerp(0f, apexBonusSpeed, apexPoint);

        float g = baseGravity;

        if (vy < 0f)
        {
            g *= fallGravityMult;
        }
        else if (vy > 0f && !jumpHeld)
        {
            g *= jumpCutMult;
        }
        else if (Mathf.Abs(vy) <= apexThreshold)
        {
            g *= apexHangMult;
        }

        rb.gravityScale = g;

        if (rb.linearVelocity.y < -maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxFallSpeed);
        }
    }



    protected override void Attacking()
    {
        fireTimer += Time.deltaTime;

        var im = InputManager4.Instance;
        bool shootHeld = im.GetShootPress();
        bool shootRelease = im.GetShoot();      

        if (state == FireMode.ProjectAngle && burstLeft > 0)
        {
            burstTimer -= Time.deltaTime;
            if (burstTimer <= 0f)
            {
                var pool = PoolingManager.Instance.GetPoolCtrl(bulletCommonPrefab);
                var rot = Quaternion.Euler(0, 0, facing == 1 ? -90f : 90f);
                pool.Spawn(bulletCommonPrefab, firePoint.position, rot);
                burstLeft--;
                burstTimer = burstInterval;
            }
        }

        if (shootHeld) fireHoldingTimer += Time.deltaTime;

        if (shootRelease)
        {
            if (fireHoldingTimer >= fireSpecialHoldingTime)
            {
                var pool2 = PoolingManager.Instance.GetPoolCtrl(bulletSpecialPrefab);
                var rot2 = Quaternion.Euler(0, 0, facing == 1 ? -90f : 90f);
                pool2.Spawn(bulletSpecialPrefab, firePoint.position, rot2);
                fireHoldingTimer = 0f;
                return;
            }

            if (state == FireMode.ProjectStraight && fireTimer >= fireCommonCooldown && burstLeft == 0)
            {
                var pool = PoolingManager.Instance.GetPoolCtrl(bulletCommonPrefab);
                var rot = Quaternion.Euler(0, 0, facing == 1 ? -90f : 90f);
                pool.Spawn(bulletCommonPrefab, firePoint.position, rot); 
                burstLeft = burstCount - 1;         
                burstTimer = burstInterval;         
                fireTimer = 0f;
                fireHoldingTimer = 0f;
                return;
            }

            if (fireTimer >= fireCommonCooldown)
            {
                var pool = PoolingManager.Instance.GetPoolCtrl(bulletCommonPrefab);
                var rot = Quaternion.Euler(0, 0, facing == 1 ? -90f : 90f);
                pool.Spawn(bulletCommonPrefab, firePoint.position, rot);
                fireTimer = 0f;
            }

            fireHoldingTimer = 0f;
        }
    }





    [System.Obsolete]
    private bool IsGrounded()
    {
        int count = groundBox.OverlapCollider(_groundFilter, groundHits);
        for (int i = 0; i < count; i++)
        {
            var h = groundHits[i];
            if (h != null && h != col && h != groundBox) return true;
        }
        return false;
    }

    private void MoveTimerCount()
    {
        bufferTimer = Mathf.Max(0f, bufferTimer - Time.deltaTime);
        coyoteTimer = Mathf.Max(0f, coyoteTimer - Time.deltaTime);
    }

}
