using UnityEngine;

public abstract class Buff : ObjectPooled
{
    [SerializeField] private Collider2D col;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (!col) col = GetComponent<Collider2D>();

    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<PlayerAbstract>(out var player))
        {
            Effecting(player);
            this.gameObject.SetActive(false);
        }
        
    }
    protected abstract void Effecting(PlayerAbstract p);
}
