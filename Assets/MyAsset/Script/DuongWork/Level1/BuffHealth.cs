using UnityEngine;

public class BuffHealth : Buff
{
    protected override void Effecting(PlayerAbstract p)
    {
        p.HealingBuff();
    }
    public override void OnSpawn()
    {
        
    }
}
