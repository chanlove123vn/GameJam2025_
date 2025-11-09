using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]
public class MeteorTrap : TrapBase
{
    [Header("Movement")]
    public float speed = 6f;                        // tốc độ rơi (đơn vị đơn trên giây)
    public float minTiltAngle = -25f;               // góc xiên hình ảnh (không ảnh hưởng chuyển động)
    public float maxTiltAngle = 25f;                
    public Vector2 angularVelocityRange = new Vector2(-200f, 200f); // tốc độ quay hình ảnh (deg/s)

    [Header("Options")]
    public bool randomizeInitialRotation = true;   

    float rotationSpeed;

    void OnEnable()
    {
        // random góc hình ảnh nhưng di chuyển thực tế sẽ luôn là xuống thẳng
        float angle = Random.Range(minTiltAngle, maxTiltAngle);
        if (randomizeInitialRotation)
            transform.rotation = Quaternion.Euler(0, 0, angle);

        rotationSpeed = Random.Range(angularVelocityRange.x, angularVelocityRange.y);
    }

    void Update()
    {
        // di chuyển thẳng xuống theo world Y (không bị ảnh hưởng bởi rotation)
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);

        // quay cho hiệu ứng hình ảnh (không đổi hướng rơi)
        if (Mathf.Abs(rotationSpeed) > 0.01f)
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    // Xử lý khi va chạm với Player (TrapBase gọi hàm này khi tag trùng)
    protected override void HandlePlayerHit(GameObject player)
    {
        DisableTrap();
    }

}