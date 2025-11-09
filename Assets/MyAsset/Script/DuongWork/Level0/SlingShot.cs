using UnityEngine;
public class SlingShot : LoadComponentMonoBehavior, IHoldPlayerInterface
{
    [SerializeField] private PlayerAbstract player;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private CircleCollider2D col;
    public Transform HoldPoint => this.holdPoint;

    [SerializeField] private float maxPull;
    [SerializeField] private float power = 60f;
    private Camera cam;
    private bool isDragging;
    private Vector2 lastOffset;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (!holdPoint) holdPoint = transform.Find("HoldPoint");
        cam = Camera.main;
        if (!col) col = GetComponent<CircleCollider2D>();
        maxPull = col.radius * transform.localScale.x;

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (player != null) return;
        var p = col.GetComponent<PlayerAbstract>();
        if (p != null)
        {
            player = p;
            player.HoldPlayer(this);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (player == null) return;
        if (col.GetComponent<PlayerAbstract>() == player && !isDragging)
        {
            player.ReleasePlayer(this);
            player = null;
        }
    }

    private void OnMouseDown()
    {
        if (player != null) isDragging = true;
    }

    private void OnMouseUp()
    {
        if (player == null || !isDragging) return;

        float pull01 = Mathf.Clamp01(lastOffset.magnitude / maxPull);
        Vector2 impulse = (-lastOffset.normalized) * (power * pull01);

        player.Launch(impulse);
        player = null;
        isDragging = false;
    }

    private void Update()
    {
        if (!isDragging || player == null) return;

        Vector3 m = cam.ScreenToWorldPoint(Input.mousePosition);
        m.z = 0f;

        Vector2 offset = (Vector2)(m - holdPoint.position);
        if (offset.magnitude > maxPull) offset = offset.normalized * maxPull;

        lastOffset = offset;

        player.DragTo(holdPoint.position + (Vector3)offset);
    }
}