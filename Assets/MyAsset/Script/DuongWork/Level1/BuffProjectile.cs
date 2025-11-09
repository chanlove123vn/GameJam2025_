using UnityEngine;

public class BuffProjectile : Buff
{
    protected override void Effecting(PlayerAbstract p)
    {
        p.ProjectileBuff();
    }
    public override void OnSpawn()
    {
        
    }
}
