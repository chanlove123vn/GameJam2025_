using UnityEngine;

public class MoveByTranslate : BaseComponent
{
    //==========================================Variable==========================================
    [Header("===Move By Translate===")]
    [SerializeField] private CBTransform mainObj;
    [SerializeField] private Float speed;
    [SerializeField] private CBVector2 dir;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref mainObj, transform.Find("Data/MainObj"), "LoadMainObj()");
        this.LoadComponent(ref speed, transform.Find("Data/Speed"), "LoadSpeed()");
        this.LoadComponent(ref dir, transform.Find("Data/Dir"), "LoadDir()");
    }

    //=======================================Base Component=======================================
    public override void OnUpdate()
    {
        base.OnUpdate();
        this.Moving();
    }

    //===========================================Method===========================================
    private void Moving()
    {
        if (mainObj == null || speed == null) return;
        mainObj.Value.Translate(dir.Value * speed.Value * Time.deltaTime, Space.World);
    }
}
