using System.Collections;
using UnityEngine;

/// <summary>
/// SeeSaw (bập bênh) — di chuyển lên/xuống và xoay trong một khoảng xác định.
/// - useLocalSpace: nếu true dùng trục local up của object, nếu false dùng world up.
/// - moveDistance: tổng chiều dài quãng đường (ví dụ 2 => đi từ -1 tới +1 quanh startPosition)
/// - speed: tốc độ di chuyển (Hz-like)
/// - rotationEnabled: bật/tắt xoay
/// - rotationSpeed: tốc độ xoay (deg/s)
/// </summary>
public class SeeSaw : TrapBase
{
    [Header("Movement")]
    [Tooltip("Tổng khoảng cách di chuyển (world units)")]
    public float moveDistance = 2f;
    [Tooltip("Tốc độ di chuyển (tăng => nhanh hơn)")]
    public float speed = 1f;
    [Tooltip("Nếu true dùng trục local up của object, nếu false dùng world up")]
    public bool useLocalSpace = true;
    [Tooltip("Bắt đầu ở giữa quãng đường nếu true, nếu false bắt đầu ở min endpoint")]
    public bool startAtCenter = true;
    [Tooltip("Nếu true dùng SmoothStep để mượt chuyển động")]
    public bool smooth = false;

    [Header("Rotation")]
    [Tooltip("Bật hiệu ứng xoay")]
    public bool rotationEnabled = true;
    [Tooltip("Tốc độ xoay (độ/giây) - số dương xoay thuận chiều kim đồng hồ")]
    public float rotationSpeed = 90f;
    [Tooltip("Trục xoay (Z cho 2D, Y cho 3D)")]
    public Vector3 rotationAxis = Vector3.forward;

    Vector3 startPosition;
    Vector3 moveDirection = Vector3.up;
    float phaseOffset = 0f; // để điều chỉnh vị trí bắt đầu
    Quaternion startRotation;

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        moveDirection = useLocalSpace ? transform.up : Vector3.up;
        if (!startAtCenter)
        {
            // offset phase để bắt đầu tại min endpoint
            phaseOffset = -moveDistance * 0.5f / Mathf.Max(0.0001f, speed);
        }
    }

    void OnEnable()
    {
        // cập nhật lại startPosition khi bật lại để căn từ vị trí hiện tại
        startPosition = transform.position;
        startRotation = transform.rotation;
        moveDirection = useLocalSpace ? transform.up : Vector3.up;
    }

    void Update()
    {
        // Di chuyển lên xuống
        float t = Mathf.PingPong((Time.time + phaseOffset) * speed, moveDistance);
        float offset = t - moveDistance * 0.5f;

        if (smooth)
        {
            // Áp dụng SmoothStep quanh khoảng -0.5..0.5 để mượt chuyển động
            float normalized = (offset / (moveDistance * 0.5f) + 1f) * 0.5f; // [0..1]
            normalized = Mathf.SmoothStep(0f, 1f, normalized);
            offset = Mathf.Lerp(-moveDistance * 0.5f, moveDistance * 0.5f, normalized);
        }

        transform.position = startPosition + moveDirection.normalized * offset;

        // Xoay nếu bật rotation
        if (rotationEnabled)
        {
            transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    // Public API: đặt điểm bắt đầu (useful nếu spawn hoặc di chuyển parent)
    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
    }

    // Public API: tạm dừng / tiếp tục (đơn giản bằng enable/disable script)
    public void PauseMovement() => enabled = false;
    public void ResumeMovement() => enabled = true;

    // TrapBase yêu cầu implement
    protected override void HandlePlayerHit(GameObject player)
    {
        // TrapBase đã xử lý damage
    }
    
    // Gizmo: vẽ đường di chuyển
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 dir = useLocalSpace ? transform.up : Vector3.up;
        Vector3 center = Application.isPlaying ? startPosition : transform.position;
        Vector3 a = center + dir.normalized * (-moveDistance * 0.5f);
        Vector3 b = center + dir.normalized * (moveDistance * 0.5f);
        Gizmos.DrawLine(a, b);
        Gizmos.DrawSphere(a, 0.05f);
        Gizmos.DrawSphere(b, 0.05f);
    }
}