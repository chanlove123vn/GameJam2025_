using UnityEngine;

public class VFXBase : ObjectPooled
{
  [SerializeField] protected ParticleSystem ps;
  [SerializeField] protected float extraLife = 0.05f;

  protected override void LoadComponent()
  {
    base.LoadComponent();
    if (!ps) ps = GetComponentInChildren<ParticleSystem>();
  }
  public override void OnSpawn()
  {
    if (!ps) ps = GetComponentInChildren<ParticleSystem>();
    float life = 0.35f;
    if (ps) { ps.Play(true); life = ps.main.duration + ps.main.startLifetime.constantMax + extraLife; }
    CancelInvoke();
    Invoke(nameof(Despawn), life);
  }
  protected virtual void Despawn()
  {
    PoolingManager.Instance.GetPoolCtrl(this).ReturnToPool(this);
  }
  public void AttachTo(Transform target)
  {
    transform.SetParent(target, false);
    transform.localPosition = Vector3.zero;

    var systems = GetComponentsInChildren<ParticleSystem>(true);
    foreach (var p in systems)
    {
      var main = p.main;
      main.simulationSpace = ParticleSystemSimulationSpace.Local;
      main.scalingMode = ParticleSystemScalingMode.Hierarchy;

      p.Clear(true);
      p.Play(true);
    }
  }
}
