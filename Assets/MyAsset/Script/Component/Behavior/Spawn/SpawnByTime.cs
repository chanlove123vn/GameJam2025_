using System.Collections.Generic;
using UnityEngine;

public class SpawnByTime : BaseComponent
{
    //==========================================Variable==========================================
    [Space(25)]
    [Header("===Shoot===")]
    [Header("Primary Value")]
    [SerializeField] private CBTransform spawnPoint;
    [SerializeField] private CBTransform prefab;
    [SerializeField] private CBCooldown Cd;
    [SerializeField] private Spawner spawner;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadValue(ref this.spawnPoint, transform.Find("Data/SpawnPoint"), "LoadSpawnPoint()");
        this.LoadValue(ref this.prefab, transform.Find("Data/Prefab"), "LoadPrefab()");
        this.LoadValue(ref this.Cd, transform.Find("Data/CD"), "LoadCd()");
    }

    //=======================================Base Component=======================================
    public override void OnUpdate()
    {
        base.OnUpdate();
        this.Shooting();
    }

    public override void OnEnd()
    {
        base.OnEnd();
        this.Cd.Value.ResetStatus();
    }

    //===========================================Method===========================================
    public void Shooting()
    {
        this.Cd.Value.CoolingDown();
        if (!this.Cd.Value.IsReady) return;

        this.Cd.Value.ResetStatus();
        var bullet = this.spawner.SpawnByObj(this.spawnPoint.Value, 
            this.spawnPoint.Value.position, this.spawnPoint.Value.rotation);
        bullet.gameObject.SetActive(true);
        
    }
}
