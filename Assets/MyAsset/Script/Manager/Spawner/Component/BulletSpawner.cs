using UnityEngine;

public class BulletSpawner : Spawner
{
    public Transform SpawnByType(BulletType bulletType, Vector3 spawnPos, Quaternion spawnRot)
    {
        Transform spawnObj = this.prefabs[(int)bulletType];
        return this.SpawnByObj(spawnObj, spawnPos, spawnRot);
    }
}
