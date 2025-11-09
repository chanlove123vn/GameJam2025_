using UnityEngine;

public class BuffProjectile : Buff
{
    protected override void Effecting(PlayerLevel1 p)
    {
        p.ProjectileBuff();
    }
    public override void OnSpawn()
    {
        
    }
}
