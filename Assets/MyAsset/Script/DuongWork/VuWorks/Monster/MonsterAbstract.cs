using UnityEngine;

public enum MonsterState
{
    ALIVE = 0,
    MOVING = 1,
    HIT = 2,
    DEAD = 3,
    IDLE = 4,
}


public abstract class MonsterAbstract : ObjectPooled
{

    //===Variables===//
    [Header("===Components===")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] protected Collider2D bodyCol;

    [Header("===Status===")]
    [SerializeField] public float moveSpeed;
    [SerializeField] protected MonsterState currentState = MonsterState.ALIVE;
    [SerializeField] public Vector2 moveDir;
    [SerializeField] protected float timer = 0;

    [Header("===HP===")]
    [SerializeField] protected float baseHP = 2f;
    protected float HP;

    //===Unity===//
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (!rb) this.rb = GetComponent<Rigidbody2D>();
        if (!bodyCol) this.bodyCol = GetComponent<Collider2D>(); 
    }
    public override void OnSpawn()
    {
        this.ResetState();
        timer = 0;
    }
    protected override void Start()
    {
        this.ResetState();
    }

    //===Method===//
    protected virtual void ResetState()
    {
        HP = baseHP;
        currentState = MonsterState.ALIVE;
    }

    public virtual void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log($"{gameObject.name} takes {damage} damage. HP: {HP}/{baseHP}");
        if (HP <= 0)
        {
            HP = 0f;
            currentState = MonsterState.DEAD;
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Monster die");
        Destroy(gameObject);
    }


    public float GetHP() => HP;
    public float GetBaseHP() => baseHP;
    public float GetHPPercent() => HP / baseHP;

    protected abstract void Moving();

}
