using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseComponent : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [Header("===Base Component===")]
    [SerializeField] private List<BaseComponent> startQueue;
    [SerializeField] private List<BaseComponent> updateQueue;
    [SerializeField] private List<BaseComponent> fixedUpdateQueue;
    [SerializeField] private List<BaseComponent> endQueue;
    [SerializeField] private List<BaseComponent> collideEnterQueue;
    [SerializeField] private List<BaseComponent> triggerEnterQueue;
    [SerializeField] private List<BaseComponent> triggerExitQueue;
    [SerializeField] private List<BaseComponent> collideExitQueue;

    //===========================================Unity============================================
    protected override void OnEnable()
    {
        base.OnEnable();
        foreach (var item in startQueue) item.OnStart();
    }

    protected virtual void Update()
    {
        foreach (var item in updateQueue)
        {
            if (item.gameObject.activeSelf) item.OnUpdate();
        }
    }

    protected virtual void FixedUpdate()
    {
        foreach (var item in fixedUpdateQueue)
        {
            if (item.gameObject.activeSelf) item.OnFixedUpdate();
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        foreach (var item in endQueue) item.OnEnd();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        foreach (var item in collideEnterQueue) item.CollisionEnter(collision);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        foreach (var item in triggerEnterQueue) item.TriggerEnter(other);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        foreach (var item in triggerEnterQueue) item.TriggerExit(other);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        foreach (var item in collideEnterQueue) item.CollisionExit(collision);
    }

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.startQueue = new List<BaseComponent>();
        this.updateQueue = new List<BaseComponent>();
        this.fixedUpdateQueue = new List<BaseComponent>();
        this.endQueue = new List<BaseComponent>();
        this.collideEnterQueue = new List<BaseComponent>();
        this.triggerEnterQueue = new List<BaseComponent>();
        this.collideExitQueue = new List<BaseComponent>();
        this.triggerExitQueue = new List<BaseComponent>();

        foreach (Transform child in transform)
        {
            var component = child.GetComponent<BaseComponent>();
            if (component == null) continue;

            this.startQueue.Add(component);
            this.updateQueue.Add(component);
            this.fixedUpdateQueue.Add(component);
            this.endQueue.Add(component);
            this.collideEnterQueue.Add(component);
            this.triggerEnterQueue.Add(component);
            this.collideExitQueue.Add(component);
            this.triggerExitQueue.Add(component);
        }
    }

    //===========================================Method===========================================
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnEnd() { }
    public virtual void CollisionEnter(Collision collision) { }
    public virtual void CollisionExit(Collision collision) { }
    public virtual void TriggerEnter(Collider other) { }
    public virtual void TriggerExit(Collider other) { }
}
