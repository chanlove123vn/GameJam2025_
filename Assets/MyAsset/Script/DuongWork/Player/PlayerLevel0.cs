using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel0 : PlayerAbstract
{
    [Header("Motion")]
    [SerializeField] private float flapUpForce = 15f; 
    [SerializeField] private float flapUpVelocity = 12f;
    [SerializeField] private float maxFallSpeed = -15f;

    [Header("Rotation")]
    [SerializeField] private float tiltFactor = 4f;
    [SerializeField] private float rotationSmoothTime = 0.1f;

    float angleVel;

    protected override void Update()
    {
        base.Update();
    }

    void FixedUpdate()
    {

        ConstantMoveRight();
    }
    protected override void InitPlayer()
    {
        baseHP = 100;
        HP = baseHP;
        speed = 10;
        dame = 0;

    }

    protected override void Moving()
    {
        if (Inputmanager1.Instance.GetJump() && !IsDead && !isLockPos) 
        {
            if (source) source.Play();
            var v = rb.linearVelocity;
            v.x = speed;
            v.y = flapUpVelocity;
            rb.linearVelocity = v;
            transform.rotation = Quaternion.Euler(0, 0, 45);
        }
    }

    void ConstantMoveRight()
    {
        if (IsDead || isLockPos) return;
        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        if (rb.linearVelocity.y < maxFallSpeed)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        float targetAngle = Mathf.Clamp(rb.linearVelocity.y * tiltFactor, -70f, 20f);
        float z = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref angleVel, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, 0f, z);
    }

    protected override void Attacking() { }

    
}
