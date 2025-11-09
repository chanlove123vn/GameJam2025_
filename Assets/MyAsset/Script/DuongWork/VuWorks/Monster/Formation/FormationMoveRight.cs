using UnityEngine;

public class FormationMoveRight : Formation
{
    protected override void StartMoving()
    {
        // Set moveDir sang phải cho tất cả enemy
        foreach (EnemyPlane enemy in enemies)
        {
            enemy.moveDir = Vector2.right;
        }
    }

    protected override void Move()
    {
        // Di chuyển parent formation
        Vector3 newPos = transform.position + Vector3.right * moveSpeed * Time.deltaTime;
        
        // Check đã tới target chưa
        if (newPos.x <= targetPos.x)
        {
            transform.position = targetPos;
            OnReachedTarget();
        }
        else
        {
            transform.position = newPos;
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
