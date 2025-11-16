using UnityEngine;

public class MoveByVel : BaseComponent
{
    //==========================================Variable==========================================
    [Header("===Move By Vel===")]
    [Header("Default Value")]
    [SerializeField] private Float defaultSpeed;
    [SerializeField] private CBVector2 defaultDir;
    [SerializeField] private Bool defaultCanMove;

    [Header("Component")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Primary Value")]
    [SerializeField] private Float speed;
    [SerializeField] private CBVector2 dir;
    [SerializeField] private Bool canMove;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();

        // Default Value
        this.LoadComponent(ref this.defaultSpeed, transform.Find("Default/Speed"), "LoadDefaultSpeed()");
        this.LoadComponent(ref this.defaultDir, transform.Find("Default/Dir"), "LoadDefaultDir()");
        this.LoadComponent(ref this.defaultCanMove, transform.Find("Default/CanMove"), "LoadDefaultCanMove()");

        // Primary Value
        this.LoadComponent(ref this.speed, transform.Find("Data/Speed"), "LoadSpeed()");
        this.LoadComponent(ref this.dir, transform.Find("Data/Dir"), "LoadDir()");
        this.LoadComponent(ref this.canMove, transform.Find("Data/CanMove"), "LoadCanMove()");
    }

    //=======================================Base Component=======================================
    public override void OnUpdate()
    {
        base.OnUpdate();
        this.Moving();
    }

    public override void OnStart()
    {
        base.OnStart();
        this.speed.Value = this.defaultSpeed.Value;
        this.dir.Value = this.defaultDir.Value;
        this.canMove.Value = this.defaultCanMove.Value;
    }

    //===========================================Method===========================================
    private void Moving()
    {
        this.rb.linearVelocity = this.dir.Value * this.speed.Value;
    }

    public void SetCanMove(bool value)
    {
        this.canMove.Value = value;
    }
}
