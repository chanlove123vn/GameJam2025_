using UnityEngine;

public class EnemyPlane : MonsterAbstract
{
    protected override void Moving()
    {
        currentState = MonsterState.MOVING;
        this.rb.linearVelocity = moveDir * this.moveSpeed;
        Debug.Log(this.transform.position);
    }

    private void Update()
    {
        this.Moving();
        this.Shooting();
    }
}
