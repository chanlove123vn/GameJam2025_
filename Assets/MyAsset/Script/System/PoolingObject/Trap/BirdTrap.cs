using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require a concrete 2D collider type (BoxCollider2D) so Unity can auto-add it
[RequireComponent(typeof(BoxCollider2D))]
public class BirdTrap : TrapBase
{
    [Header("Flight")]
    public float flightSpeed = 3f;
    public bool moveRightToLeft = true;
    public bool useAnimator = false;
    public Animator animator;

    [Header("Drop")]
    [Tooltip("Khoảng thời gian ngẫu nhiên giữa các lần thả (s)")]
    public float minDropInterval = 4f;
    public float maxDropInterval = 5f;
    public Vector3 dropOffset = Vector3.zero;

    [Tooltip("List các trap prefab có thể thả — mỗi lần sẽ chọn ngẫu nhiên 1 trap từ list này")]
    [SerializeField] private List<TrapBase> trapsDrop = new();
    [SerializeField] private Transform DropPos;

    float dir = -1f;
    private Cooldown dropCooldown;

    void Awake()
    {
        dir = moveRightToLeft ? -1f : 1f;

        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }

        // Khởi tạo cooldown với thời gian ngẫu nhiên ban đầu
        float initialCD = Random.Range(minDropInterval, maxDropInterval);
        dropCooldown = new Cooldown(initialCD);
    }

    void Update()
    {
        if (!useAnimator)
        {
            transform.Translate(Vector3.right * dir * flightSpeed * Time.deltaTime, Space.World);
        }

        // Cập nhật cooldown và thả đồ khi ready
        UpdateDrop();
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (!DropPos) DropPos = transform.Find("DropPos");
    }

    void OnEnable()
    {
        // Reset cooldown khi bird được enable
        if (dropCooldown != null)
        {
            float newCD = Random.Range(minDropInterval, maxDropInterval);
            dropCooldown.TimeLimit = newCD;
            dropCooldown.ResetStatus();
        }
    }

    void OnDisable()
    {
        // Có thể reset cooldown ở đây nếu cần
        if (dropCooldown != null)
        {
            dropCooldown.ResetStatus();
        }
    }

    protected override void HandlePlayerHit(GameObject player)
    {
        /*
        player.Deduct(1);
        */
    }

    void UpdateDrop()
    {
        if (trapsDrop == null || trapsDrop.Count == 0) return;
        if (dropCooldown == null) return;

        // Cập nhật cooldown
        dropCooldown.CoolingDown();

        // Khi cooldown ready, thả 1 trap ngẫu nhiên
        if (dropCooldown.IsReady)
        {
            DropRandomTrap();

            // Reset cooldown với thời gian ngẫu nhiên mới
            float newCD = Random.Range(minDropInterval, maxDropInterval);
            dropCooldown.TimeLimit = newCD;
            dropCooldown.ResetStatus();
        }
    }

    void DropRandomTrap()
    {
        // Chọn ngẫu nhiên 1 trap từ list
        int randomIndex = Random.Range(0, trapsDrop.Count);
        TrapBase selectedTrap = trapsDrop[randomIndex];

        if (selectedTrap == null) return;

        // Tính vị trí thả
        Vector3 spawnPos = (DropPos != null ? DropPos.position : transform.position) + dropOffset;

        // Spawn trap qua pooling
        var pool = PoolingManager.Instance?.GetPoolCtrl(selectedTrap);
        if (pool != null)
        {
            pool.Spawn(selectedTrap, spawnPos, Quaternion.identity);
        }
        else
        {
            // Fallback: instantiate trực tiếp
            Instantiate(selectedTrap.gameObject, spawnPos, Quaternion.identity);
        }
    }
}