using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class TrapBase : ObjectPooled
{
    [Tooltip("Tag của đối tượng mục tiêu, ví dụ Player")]
    public string targetTag = "Player";

    [Header("Damage")]
    [Tooltip("Số HP trừ khi va chạm (positive value). Đặt 0 để không gây sát thương")]
    public int damage = 1; // mặc định trừ 1 HP

    // Thực thi kiểm tra chung cho Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            HandlePlayerHit(other.gameObject);
            PoolingManager.Instance.GetPoolCtrl(this).ReturnToPool(this);
        }
    }

    // Gom: áp dụng damage (nếu có) rồi gọi xử lý cụ thể của trap
    protected abstract void HandlePlayerHit(GameObject player);
    public override void OnSpawn()
    {
        
    }
    public virtual void ResetTrap()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisableTrap()
    {
        gameObject.SetActive(false);
    }
}