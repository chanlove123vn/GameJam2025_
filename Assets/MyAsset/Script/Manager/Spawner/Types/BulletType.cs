using UnityEngine;

public enum BulletType
{
    NORMAL_LEVEL_1 = 0,
    ROCKET_LEVEL_1 = 1,
}

public class CBBulletType : PrimaryValue
{
    public BulletType Value;
}
