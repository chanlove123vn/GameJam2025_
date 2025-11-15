using UnityEngine;

public class Despawner : BaseComponent
{
    //==========================================Variable==========================================
    [Space(25)]
    [Header("===Despawner===")]
    [Header("Primary Value")]
    [SerializeField] private Spawner spawner;
    [SerializeField] private CBUnityEvent onDespawn;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.onDespawn, transform.Find("Data/OnDespawn"), "LoadOnDespawn()");
    }

    //===========================================Method===========================================
    public void DespawnObj(Transform obj)
    {
        this.spawner.Despawn(obj);
        if (this.onDespawn != null) this.onDespawn.Value?.Invoke();
    }
}
