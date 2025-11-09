using UnityEngine;

public enum FormationDirection
{
    Right,
    Left,
    Down
}

public class Formation : ObjectPooled
{
    [SerializeField] private float moveSpeed;
    [SerializeField] public FormationDirection direction = FormationDirection.Right;
    [SerializeField] public Vector2 moveDir;
    
    private EnemyPlane[] enemies;
    private Vector3 targetPos;
    private bool isMoving = false;

    public void Start()
    {
        OnStart();
    }

    public void SetTarget(Vector3 target)
    {
        targetPos = target;
        isMoving = true;
        StartMoving();
    }

    public void OnStart()
    {
        enemies = GetComponentsInChildren<EnemyPlane>();
        moveSpeed = enemies.Length > 0 ? enemies[0].moveSpeed : 5f;
        moveDir = GetMoveDirectionFromEnum();
    }

    private Vector2 GetMoveDirectionFromEnum()
    {
        switch (direction)
        {
            case FormationDirection.Right:
                return Vector2.right;
            case FormationDirection.Left:
                return Vector2.left;
            case FormationDirection.Down:
                return Vector2.down;
            default:
                return Vector2.right;
        }
    }

    public void StartMoving()
    {
        foreach (EnemyPlane enemy in enemies)
        {
            enemy.moveDir = Vector2.zero;
        }
    }

    public void Update()
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

    public void Move()
    {
        Vector3 currentCenter = GetFormationBounds().center;
        Vector3 movement = Vector3.zero;
        bool reached = false;

        if (direction == FormationDirection.Right)
        {
            movement = Vector3.right * moveSpeed * Time.deltaTime;
            if (currentCenter.x + moveSpeed * Time.deltaTime >= targetPos.x)
                reached = true;
        }
        else if (direction == FormationDirection.Left)
        {
            movement = Vector3.left * moveSpeed * Time.deltaTime;
            if (currentCenter.x - moveSpeed * Time.deltaTime <= targetPos.x)
                reached = true;
        }
        else if (direction == FormationDirection.Down)
        {
            movement = Vector3.down * moveSpeed * Time.deltaTime;
            if (currentCenter.y - moveSpeed * Time.deltaTime <= targetPos.y)
                reached = true;
        }

        if (reached)
        {
            Vector3 offset = transform.position - currentCenter;
            transform.position = targetPos + offset;
            OnReachedTarget();
        }
        else
        {
            transform.position += movement;
        }
    }

    public void OnReachedTarget()
    {
        isMoving = false;
        StopAll();
    }

    public void StopAll()
    {
        foreach (EnemyPlane enemy in enemies)
        {
            enemy.moveDir = Vector2.zero;
            if (enemy.rb != null)
                enemy.rb.linearVelocity = Vector2.zero;
        }
    }

    public override void OnSpawn()
    {
        isMoving = false;
        
        if (enemies == null || enemies.Length == 0)
        {
            enemies = GetComponentsInChildren<EnemyPlane>();
        }
        
        foreach (EnemyPlane enemy in enemies)
        {
            enemy.moveDir = Vector2.zero;
            if (enemy.rb != null)
                enemy.rb.linearVelocity = Vector2.zero;
        }
    }
}
