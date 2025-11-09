using UnityEngine;

public class MoveByVel : BaseComponent
{
    //==========================================Variable==========================================
    [Header("===Move By Vel===")]
    [Header("Component")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Priamry Value")]
    [SerializeField] private Float speed;
    [SerializeField] private CBVector2 dir;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.speed, transform.Find("Data/Speed"), "LoadSpeed()");
        this.LoadComponent(ref this.dir, transform.Find("Data/Dir"), "LoadDir()");
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
        this.rb.linearVelocity = this.dir.Value * this.speed.Value;
    }
}
