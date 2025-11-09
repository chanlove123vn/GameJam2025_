// CameraFollow2D.cs
using UnityEngine;

[DisallowMultipleComponent]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Offsets")]
    public float offsetX = 3f;   
    public float offsetY = 0f;      

    [Header("Behavior")]
    public bool followY = false;    
    public float smoothTime = 0.12f;
    public float minX = -1000f;     

    Vector3 velocity; 

    void LateUpdate()
    {
        if (!target) return;

        float targetX = Mathf.Max(minX, target.position.x + offsetX);
        float targetY = followY ? target.position.y + offsetY : transform.position.y;

        Vector3 goal = new Vector3(targetX, targetY, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }
}
