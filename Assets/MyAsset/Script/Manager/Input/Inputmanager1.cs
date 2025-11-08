using UnityEngine;

public class Inputmanager1 : Singleton<Inputmanager1>
{
    //==========================================Variable==========================================
    [Header("===Input Manager 1===")]
    // Jump
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    // Mouse Detect
    private Camera mainCamera;
    [SerializeField] private LayerMask detectableLayers;
    [SerializeField] private string[] detectableTags;

    //===========================================Unity============================================
    public void Update()
    {
        this.DetectingByMouse();
    }

    protected override void Awake()
    {
        this.mainCamera = Camera.main;
    }

    //===========================================Method===========================================
    public bool GetJump() => Input.GetKey(jumpKey);

    private void DetectingByMouse()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, this.detectableLayers)) return;
            
        Transform target = hit.transform;
        if (this.detectableTags == null || this.detectableTags.Length < 0) return;
        foreach (var tag in this.detectableTags)
        {
            if (!target.CompareTag(tag)) continue;
            Debug.Log($"[Inputmanager1] Hit object with tag '{tag}': {target.name}");
            return;
        }
        
        Debug.Log($"[Inputmanager1] Hit object on layer '{LayerMask.LayerToName(target.gameObject.layer)}': {target.name}");
    }
}
