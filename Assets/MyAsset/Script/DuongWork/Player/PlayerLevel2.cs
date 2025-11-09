using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel2 : PlayerAbstract
{
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private float speedUp;
    [SerializeField] private float speedDown;
    [SerializeField] private float timer;

    public float groundCheckRadius = 0.05f;
    // Jump feel
    [SerializeField] private float coyoteTime = 0.10f;
    [SerializeField] private float jumpBuffer = 0.10f;
    [SerializeField, Range(0f, 1f)] private float jumpCut = 0.5f;

    private float coyoteTimer;
    private float bufferTimer;
    [SerializeField] private float jumpPower;
    public LayerMask groundLayer;

    protected override void Update()
    {
        base.Update();

        MoveTimerCount();

        Jumping();
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
    }
    protected override void InitPlayer()
    {
        baseHP = 100;
        HP = baseHP;
        speed = 10;
        dame = 0;
        speedUp = 0.8f;
        jumpPower = 7f;
        speedDown = 1.5f;
    }

    protected override void Moving()
    {
        if (IsDead || isLockPos) return;
        if (!IsGrounded()) return;
        moveInput = InputManager3.Instance.GetMoveDir();

        float maxSpeed = speed;
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
    }



    protected override void Attacking()
    {

    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(col.bounds.center + Vector3.down * (col.bounds.extents.y + groundCheckRadius), groundCheckRadius, groundLayer);
    }

    private void MoveTimerCount()
    {
        bufferTimer = Mathf.Max(0f, bufferTimer - Time.deltaTime);
        coyoteTimer = Mathf.Max(0f, coyoteTimer - Time.deltaTime);
    }

}
