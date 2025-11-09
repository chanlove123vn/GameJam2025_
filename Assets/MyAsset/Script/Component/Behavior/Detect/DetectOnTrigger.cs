using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DetectOnTrigger : BaseComponent
{
    [Header("===Detect On Trigger===")]
    [Header("Priamry Value")]
    [SerializeField] private CBLayerMask layer;
    [SerializeField] private List<String> tags;
    [SerializeField] private CBUnityEvent onTrigger;

    public override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if (this.layer.Value == other.gameObject.layer) return;

        if (this.tags != null && this.tags.Count > 0) return;
        foreach (var tag in this.tags)
        {
            if (other.tag != tag.Value) continue;
        }

        this.Collide(other);
    }

    private void Collide(Collider other)
    {
        this.onTrigger.Value?.Invoke();
    }
}
