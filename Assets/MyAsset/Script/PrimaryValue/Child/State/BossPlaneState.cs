using UnityEngine;

public enum BossPlaneState
{
    APPEAR = 0,
    ON_MAP = 1,
    DEAD = 2,
}

public class CBBossPlaneState : PrimaryValue
{
    public BossPlaneState Value;
}
