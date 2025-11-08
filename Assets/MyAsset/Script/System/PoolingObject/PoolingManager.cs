using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolingManager : Singleton<PoolingManager>
{
    [SerializeField] private List<PoolCrlBase> poolCtrlList = new List<PoolCrlBase>();

    protected override void LoadComponent()
    {
        base.LoadComponent();
        LoadPoolCtrlList();
    }
    private void LoadPoolCtrlList()
    {
        poolCtrlList.Clear();
        var list = GetComponentsInChildren<PoolCrlBase>(true);
        foreach (var b in list)
        {
            poolCtrlList.Add(b);
        }
    }
    public PoolCtrl<T> GetPoolCtrl<T>(T instance) where T : ObjectPooled
    {
        if (instance == null) return null;

        foreach (var pool in poolCtrlList)
        {
            PoolCrlBase correctPool = pool as PoolCtrl<T>;
            if (correctPool != null)
            {
                return correctPool as PoolCtrl<T>;
            }
        }

        Debug.LogWarning($"[PoolingObjManagement] Không tìm thấy PoolCtrl phù hợp cho {typeof(T).Name}");
        return null;
    }
    public void ResetAllPools()
    {
        LoadPoolCtrlList();
        foreach (var pool in poolCtrlList)
            pool.ResetPool();
    }
}

