using UnityEngine;

public class TriggerByDistance : BaseComponent
{
    //==========================================Variable==========================================
    [Header("===Trigger By Time===")]
    [Header("Primary Value")]
    [SerializeField] private CBTransform mainObj;
    [SerializeField] private CBTransform target;
    [SerializeField] private Float distance;
    [SerializeField] private CBUnityEvent trigger;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadValue(ref this.mainObj, transform.Find("Data/MainObj"), "LoadMainObj()");
        this.LoadValue(ref this.target, transform.Find("Data/Target"), "LoadTarget()");
        this.LoadValue(ref this.trigger, transform.Find("Data/Trigger"), "LoadTrigger()");
        this.LoadValue(ref this.distance, transform.Find("Data/Distance"), "LoadDistance()");
    }

    //=======================================Base Component=======================================
    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        this.Checking();
    }

    //===========================================Method===========================================
    private void Checking()
    {
        float distance = Vector2.Distance(this.mainObj.Value.position, this.target.Value.position);

        if (distance > this.distance.Value) return;
        this.trigger.Value?.Invoke();
    }
}
