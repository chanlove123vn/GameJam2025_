using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : BaseComponent
{
    //==========================================Variable==========================================
    [Space(25)]
    [Header("===Shoot===")]
    [Header("Primary Value")]
    [SerializeField] private List<CBTransform> spawnPoints;
    [SerializeField] private CBCooldown reloadCd;
    [SerializeField] private CBBulletType bullet;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadValue(ref this.spawnPoints, transform.Find("Data/ShootPoints"), "LoadSpawnPoints()");
        this.LoadValue(ref this.bullet, transform.Find("Data/Bullet"), "LoadBullet()");
        this.LoadValue(ref this.reloadCd, transform.Find("Data/ReloadCD"), "LoadReloadCd()");
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
        this.reloadCd.Value.ResetStatus();
    }

    //===========================================Method===========================================
    public void Shooting()
    {
        this.reloadCd.Value.CoolingDown();

        if (!this.reloadCd.Value.IsReady) return;
        this.reloadCd.Value.ResetStatus();
        
        foreach (var point in this.spawnPoints)
        {
            var bullet = SpawnerManager.Instance.Bullet.SpawnByType(this.bullet.Value, point.Value.position, point.Value.rotation);
            bullet.gameObject.SetActive(true);
        }
    }
}
