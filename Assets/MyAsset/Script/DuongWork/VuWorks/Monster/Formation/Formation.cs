using UnityEngine;

public abstract class Formation : ObjectPooled
{
    [SerializeField] protected float moveSpeed;
    
    protected EnemyPlane[] enemies;
    protected Vector3 targetPos;
    protected bool isMoving = false;

    protected virtual void Start()
    {
        enemies = GetComponentsInChildren<EnemyPlane>();
        moveSpeed = enemies.Length > 0 ? enemies[0].moveSpeed : 5f;
    }

    public virtual void SetTarget(Vector3 target)
    {
        targetPos = target;
        isMoving = true;
        StartMoving();
    }

    protected abstract void StartMoving();

    protected virtual void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    public Bounds GetFormationBounds()
    {
        if (enemies.Length == 0)
            return new Bounds(transform.position, Vector3.zero);
        
        Bounds bounds = new Bounds(enemies[0].transform.position, Vector3.zero);
        
        foreach (EnemyPlane enemy in enemies)
        {
            bounds.Encapsulate(enemy.transform.position);
        }
        
        return bounds;
    }

    protected abstract void Move();

    protected virtual void OnReachedTarget()
    {
        isMoving = false;
        StopAll();
    }

    protected void StopAll()
    {
        foreach (EnemyPlane enemy in enemies)
        {
            enemy.moveDir = Vector2.zero;
            if (enemy.rb != null)
                enemy.rb.linearVelocity = Vector2.zero;
        }
    }

    public void DeactivateFormation()
    {
        gameObject.SetActive(false);
    }

}
