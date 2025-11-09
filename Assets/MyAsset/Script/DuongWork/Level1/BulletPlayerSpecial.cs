using UnityEngine;


public class BulletPlayerSpecial : BulletPlayerLv1
{
   protected override void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent<MonsterAbstract>(out var monster))
        {
            monster.TakeDamage(5);
        }
        
    }

}