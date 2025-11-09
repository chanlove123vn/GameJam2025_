using UnityEngine;

public class BuffProjectileTripleAngle : Buff
{
    protected override void Effecting(PlayerAbstract p)
    {
        p.ProjectileBuffTrippleAngle();
    }
    public override void OnSpawn()
    {
        
    }
}
