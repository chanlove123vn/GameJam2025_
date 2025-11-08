using UnityEngine;

public class InputManager3 : Singleton<InputManager3>
{
    //==========================================Variable==========================================
    [Header("===Input Manager 3===")]
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    //===========================================Method===========================================
    public Vector2 GetMoveDir()
    {
        float xDir = Input.GetKey(moveRightKey) ? 1 : Input.GetKey(moveLeftKey) ? -1 : 0;
        float yDir = Input.GetKey(moveUpKey) ? 1 : Input.GetKey(moveDownKey) ? -1 : 0;
        return new Vector2(xDir, yDir).normalized;
    }

    public bool GetJump() => Input.GetKey(jumpKey);
}
