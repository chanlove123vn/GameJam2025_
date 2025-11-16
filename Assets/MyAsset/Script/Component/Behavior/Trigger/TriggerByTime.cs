using UnityEngine;

public class TriggerByTime : BaseComponent
{
    //==========================================Variable==========================================
    [Header("===Trigger By Time===")]
    [Header("Primary Value")]
    [SerializeField] private CBCooldown cd;
    [SerializeField] private CBUnityEvent trigger;
    [SerializeField] private Bool canLoop;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.cd, transform.Find("Data/CD"), "LoadCD()");
        this.LoadComponent(ref this.trigger, transform.Find("Data/Trigger"), "LoadTrigger()");
        this.LoadValue(ref this.canLoop, transform.Find("Data/CanLoop"), "LoadCanLoop()");
    }

    //=======================================Base Component=======================================
    public override void OnStart()
    {
        base.OnStart();
        this.cd.Value.ResetStatus();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.Counting();
    }

    //===========================================Method===========================================
    private void Counting()
    {
        this.cd.Value.CoolingDown();
        if (!this.cd.Value.IsReady) return;
        
        this.trigger.Value?.Invoke();
        if (!this.canLoop.Value) return;
        
        this.cd.Value.ResetStatus();
    }
}
