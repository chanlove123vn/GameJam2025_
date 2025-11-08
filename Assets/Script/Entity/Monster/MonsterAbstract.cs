using UnityEngine;

public enum MonsterState
{
    ALIVE = 0,
    MOVING = 1,
    HIT = 2,
    DEAD = 3,
}


public abstract class MonsterAbstract : HuyMonoBehaviour
{
    [Header("===Components===")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D bodyCol;

    [Header("===Status===")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected MonsterState currentState = MonsterState.ALIVE;
    [SerializeField] protected Vector2 moveDir;

    [Header("===Shotting===")]
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float timer = 0;

    [Header("===HP===")]
    [SerializeField] protected float baseHP = 5f;
    protected float HP;

    //===Unity===//
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.firePoint, transform, "FirePoint");
    }

    protected virtual void Start()
    {
        this.ResetHP();
    }

    protected virtual void ResetHP()
    {
        HP = baseHP;
    }

    public virtual void TakeDamage(float damage)
    {
        HP -= damage;
        Debug.Log($"{gameObject.name} takes {damage} damage. HP: {HP}/{baseHP}");
        if (HP <= 0)
        {
            this.Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(this.gameObject);
    }

    public float GetHP() => HP;
    public float GetBaseHP() => baseHP;
    public float GetHPPercent() => HP / baseHP;

    protected abstract void Moving();

}
