using UnityEngine;

public class BuffHealth : Buff
{
    protected override void Effecting(PlayerLevel1 p)
    {
        p.HealingBuff();
    }
    public override void OnSpawn()
    {
        
    }
}
