using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPooled : LoadComponentMonoBehavior
{

    public string GetName()
    {
        return this.gameObject.name;
    }
    public abstract void OnSpawn();
    public virtual void OnDespawn()
    {}

}
