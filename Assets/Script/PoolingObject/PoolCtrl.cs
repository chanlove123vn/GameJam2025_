using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolCtrl<T> : PoolCrlBase where T : ObjectPooled
{
    [SerializeField] private List<T> poolList = new List<T>();

    [SerializeField] private PoolHolder holder;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        LoadHolder();
    }

    private void LoadHolder()
    {
        if (holder != null) return;
        holder = transform.GetComponentInChildren<PoolHolder>();
        if (holder == null)
        {
            GameObject holderfolder = new GameObject("PoolHolder");
            holderfolder.transform.parent = this.transform;
            holderfolder.AddComponent<PoolHolder>();
            holder = holderfolder.GetComponent<PoolHolder>();
        }
    }

    public GameObject Spawn(T prefab, Vector3 location, Quaternion rotation)
    {
        T inst = GetObjectFromList(prefab);
        GameObject go;

        if (inst != null)
        {
            go = inst.gameObject;
        }
        else
        {
            go = Instantiate(prefab.gameObject, holder.transform);
            inst = go.GetComponent<T>();
            if (inst == null) inst = go.AddComponent<T>();
        }

        go.name = prefab.GetName();
        go.transform.SetPositionAndRotation(location, rotation);

        var rb2d = go.GetComponent<Rigidbody2D>(); if (rb2d) { rb2d.linearVelocity = Vector2.zero; rb2d.angularVelocity = 0f; }
        var rb = go.GetComponent<Rigidbody>(); if (rb) { rb.linearVelocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }

        go.SetActive(true);
        inst.OnSpawn();
        return go;
    }

    public void ReturnToPool(T prefab)
    {
        AddToPool(prefab);
        prefab.transform.parent = holder.transform;
        prefab.gameObject.SetActive(false);

    }

    private T GetObjectFromList(T obj)
    {
        foreach (var poolingobj in poolList)
        {
            if (poolingobj.GetName() == obj.GetName())
            {
                RemoveFromPool(poolingobj);
                return poolingobj;
            }
        }
        return null;
    }

    private void RemoveFromPool(T obj)
    {
        if (!poolList.Contains(obj) || poolList.Count == 0) return;
        poolList.Remove(obj);
    }

    private void AddToPool(T obj)
    {
        poolList.Add(obj);
    }

    public override void ResetPool()
    {
        if (holder == null) return;

        poolList.Clear();

        for (int i = holder.transform.childCount - 1; i >= 0; i--)
        {
            var child = holder.transform.GetChild(i);
            var item = child.GetComponent<T>();
            if (item == null) continue;

            if (child.gameObject.activeSelf)
            {
                ReturnToPool(item);
            }
            else
            {

                ReturnToPool(item);
            }
        }
    }
}

