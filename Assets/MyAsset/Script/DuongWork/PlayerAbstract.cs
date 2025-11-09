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
        if (!sprite) sprite = GetComponent<SpriteRenderer>();
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
        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
        transform.position = hold.HoldPoint.position;
    }
    public void ReleasePlayer(IHoldPlayerInterface hold)
    {
        isLockPos = false;
        rb.isKinematic = false;
    }

    public void DragTo(Vector3 worldPos)
    {
        if (isLockPos) transform.position = worldPos;
    }

    public void Launch(Vector2 impulse)
    {
        isLockPos = false;
        rb.isKinematic = false;
        rb.AddForce(impulse, ForceMode2D.Impulse);
    }
    public virtual void Die()
    {
        if (IsDead) return;
        IsDead = true;
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    public void Deduct(int damage)
    {
        HP -= damage;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<MonsterAbstract>(out var monsterAbstract))
        {
            Deduct(1);
        }
    }



}
