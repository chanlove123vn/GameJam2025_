using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BossPlane1 : BaseComponent
{
    //==========================================Variable==========================================
    [Header("===Boss Plane 1===")]
    [Header("State")]
    [SerializeField] private List<BaseComponent> appearQueue;
    [SerializeField] private List<BaseComponent> onMapQueue;
    [SerializeField] private List<BaseComponent> deadQueue;

    [Header("Component")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D bodyCol;

    [Header("Priamry Value")]
    [SerializeField] private CBBossPlaneState state;
    [SerializeField] private Int maxHealth;
    [SerializeField] private Int currHealth;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        // Component
        this.LoadComponent(ref this.rb, transform, "LoadRigidBody2D()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadCapsuleCollider2D()");

        // State
        this.LoadComponent(ref this.appearQueue, transform.Find("State/Appear"), "LoadAppearQueue()");
        this.LoadComponent(ref this.onMapQueue, transform.Find("State/OnMap"), "LoadOnMapQueue()");
        this.LoadComponent(ref this.deadQueue, transform.Find("State/Dead"), "LoadDeadQueue()");

        // Primary Value
        this.LoadValue(ref this.state, transform.Find("Data/State"), "LoadState()");
        this.LoadValue(ref this.maxHealth, transform.Find("Data/MaxHealth"), "LoadMaxHealth()");
        this.LoadValue(ref this.currHealth, transform.Find("Data/CurrHealth"), "LoadCurrHealth()");
    }

    //===========================================Unity============================================
    protected override void Update()
    {
        base.Update();
        switch (this.state.Value)
        {
            case BossPlaneState.APPEAR:
                foreach (var item in this.appearQueue) item.OnUpdate();
                break;
            case BossPlaneState.ON_MAP:
                foreach (var item in this.onMapQueue) item.OnUpdate();
                break;
            case BossPlaneState.DEAD:
                foreach (var item in this.deadQueue) item.OnUpdate();
                break;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        switch (this.state.Value)
        {
            case BossPlaneState.APPEAR:
                foreach (var item in this.appearQueue) item.OnFixedUpdate();
                break;
            case BossPlaneState.ON_MAP:
                foreach (var item in this.onMapQueue) item.OnFixedUpdate();
                break;
            case BossPlaneState.DEAD:
                foreach (var item in this.deadQueue) item.OnFixedUpdate();
                break;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (var item in this.appearQueue) item.OnStart();
        foreach (var item in this.onMapQueue) item.OnStart();
        foreach (var item in this.deadQueue) item.OnStart();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var item in this.appearQueue) item.OnEnd();
        foreach (var item in this.onMapQueue) item.OnEnd();
        foreach (var item in this.deadQueue) item.OnEnd();
    }

    //===========================================Method===========================================
    public void SwitchToOnMap()
    {
        this.state.Value = BossPlaneState.ON_MAP;
    }

    public void StopMove()
    {
        this.rb.linearVelocity = Vector2.zero;
    }
}
