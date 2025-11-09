using UnityEngine;

public abstract class PlayerAbstract : LoadComponentMonoBehavior
{
    
    public int HP;
    public int baseHP = 3;
    [SerializeField] protected float speed;
    protected int dame;
    public bool IsDead { get; private set; }
    protected bool isLockPos = false;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected AudioSource source;
    [SerializeField] protected Collider2D col;
    [SerializeField] protected Animator anim;
    [SerializeField] protected SpriteRenderer sprite;
    protected int facing = 1;
    

    public enum FireMode { Normal, Project }
    [Header("Projectile Buff")]
    [SerializeField] protected FireMode state = FireMode.Normal;
    [SerializeField] protected float projectileBuffDuration = 10f;
    protected float projectileBuffTimer;
    [SerializeField] protected float spreadAngle = 30f; 


    protected virtual void Update()
    {
        Moving();
        Attacking();
    }
    protected override void Awake()
    {
        base.Awake();
        InitPlayer();
    }
    protected override void LoadComponent()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        IsDead = false;
        if (!col) col = GetComponent<Collider2D>();
        if (!anim) anim = GetComponent<Animator>();
        if(!sprite) sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void InitPlayer()
    {
        HP = baseHP;
        baseHP = 3;
    }
    
    protected abstract void Attacking();
    protected abstract void Moving();
    public void HoldPlayer(IHoldPlayerInterface hold)
    {
        isLockPos = true;
        var holdobj = hold as MonoBehaviour;
        transform.position = holdobj.transform.position;
    }
    public void ReleasePlayer(IHoldPlayerInterface hold)
    {
        isLockPos = false;
    }
    public virtual void Die()
    {
        if (IsDead) return;
        IsDead = true;
        rb.linearVelocity = Vector2.zero;
    }

    public void Deduct()
    {
        HP -= 1;
        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
    }
    public void HealingBuff()
    {
        if (HP >= baseHP) return;
        HP += 1;
    }

    public void ProjectileBuff()
    {
        state = FireMode.Project;
        projectileBuffTimer = projectileBuffDuration;
    }


}
