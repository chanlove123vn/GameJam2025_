using System;
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
    [Header("Masks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask nextLevelMask;
    public event Action<PlayerAbstract> OnGoNextLevel;

    [Header("Camera switch")]
    [SerializeField] private CameraFollow cam;
    [SerializeField] private Transform posLv2;
    [SerializeField] private Vector2 spawnPos;

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
        base.InitPlayer();
        speed = 10;
        dame = 0;
        spawnPos = transform.position;
    }

    protected override void Moving()
    {
        // if (Inputmanager1.Instance.GetJump() && !IsDead && !isLockPos) 
        // {
        //     if (source) source.Play();
        //     var v = rb.linearVelocity;
        //     v.x = speed;
        //     v.y = flapUpVelocity;
        //     rb.linearVelocity = v;
        //     transform.rotation = Quaternion.Euler(0, 0, 45);
        // }
    }

    void ConstantMoveRight()
    {
        // if (IsDead || isLockPos) return;
        // rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        // if (rb.linearVelocity.y < maxFallSpeed)
        //     rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        // float targetAngle = Mathf.Clamp(rb.linearVelocity.y * tiltFactor, -50f, 20f);
        // float z = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref angleVel, rotationSmoothTime);
        // transform.rotation = Quaternion.Euler(0f, 0f, z);
    }

    protected override void Attacking() { }

    void OnTriggerEnter2D(Collider2D other) => HandleHit(other.gameObject);
    void OnCollisionEnter2D(Collision2D other) => HandleHit(other.gameObject);
    void HandleHit(GameObject go)
    {
        int layer = go.layer;

        if ((groundLayer.value & (1 << layer)) != 0)
        {
            transform.position = spawnPos;
            return;
        }

        if ((nextLevelMask.value & (1 << layer)) != 0)
        {
            if (cam && posLv2) cam.target = posLv2;
            OnGoNextLevel?.Invoke(this);
            gameObject.SetActive(false);

        }
    }
}
