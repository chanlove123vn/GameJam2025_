using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DirectToTarget : BaseComponent
{
    //==========================================Variable==========================================
    [Space(25)]
    [Header("===Direct To Target===")]
    [Header("Primary Value")]
    [SerializeField] private CBTransform mainObj;
    [SerializeField] private CBTransform target;

    [Header("Reference")]
    [SerializeField] private List<CBTransform> refObjs;
    [SerializeField] private List<CBVector2> refDir2ds;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.mainObj, transform.Find("Data/MainObj"), "LoadMainObj()");
        this.LoadComponent(ref this.target, transform.Find("Data/Target"), "LoadTarget()");
    }

    //=======================================Base Component=======================================
    public override void OnUpdate()
    {
        base.OnUpdate();
        this.Directing();
    }

    //===========================================Method===========================================
    private void Directing()
    {
        Vector3 dir = (this.target.Value.position - this.mainObj.Value.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        foreach (CBTransform obj in this.refObjs)
        {
            Vector3 currentEuler = obj.Value.rotation.eulerAngles;
            obj.Value.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, angle);
        }


        foreach (CBVector2 dir2D in this.refDir2ds)
        {
            dir2D.Value = dir;
        }
    }
}
