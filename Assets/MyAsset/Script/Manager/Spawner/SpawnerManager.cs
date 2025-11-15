using UnityEngine;

public class SpawnerManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static SpawnerManager instance;
    public static SpawnerManager Instance => instance;

    [SerializeField] private BackgroundSpawner background;
    public BackgroundSpawner Background => background;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("SpawnerManager is exist", this.gameObject); 
            return;
        }

        instance = this;
        base.Awake();
    }

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.background, transform.Find("Background"), "LoadBackground()");
    }
}
