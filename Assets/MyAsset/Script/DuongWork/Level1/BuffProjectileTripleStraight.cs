using UnityEngine;

public class BuffProjectileTripleStraight : Buff
{
    protected override void Effecting(PlayerAbstract p)
    {
        p.ProjectileBuffTrippleStraight();
    }
    public override void OnSpawn()
    {
        
    }
}
