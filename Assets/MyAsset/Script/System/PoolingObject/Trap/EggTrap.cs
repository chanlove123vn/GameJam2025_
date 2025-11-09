using UnityEngine;

// đảm bảo có 1 collider cụ thể để Unity auto-add (tránh dùng abstract Collider2D)
[RequireComponent(typeof(CircleCollider2D))]
// Egg rơi xuống, va chạm với player sử dụng TrapBase
public class EggTrap : TrapBase
{
    [Header("Fall")]
    public float fallSpeed = 4f; // set trong Inspector

    [Header("Variants (visual only)")]
    [Tooltip("Nếu sử dụng sprites -> egg sẽ đổi sprite ngẫu nhiên khi active.")]
    public Sprite[] variantSprites;

    SpriteRenderer sr;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // Only change sprite variants here — do NOT instantiate other prefabs.
        if (variantSprites != null && variantSprites.Length > 0 && sr != null)
        {
            sr.sprite = variantSprites[Random.Range(0, variantSprites.Length)];
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
    }

    protected override void HandlePlayerHit(GameObject player)
    {
        /*
        playerhit -hp
        */
        OnDespawn();
    }
}