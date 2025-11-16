using UnityEngine;

public class SpawnerManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static SpawnerManager instance;
    public static SpawnerManager Instance => instance;

    [SerializeField] private BackgroundSpawner background;
    [SerializeField] private BulletSpawner bullet;
    [SerializeField] private EntitySpawner entity;

    public BackgroundSpawner Background => background;
    public BulletSpawner Bullet => bullet;
    public EntitySpawner Entity => entity;

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
        this.LoadComponent(ref this.bullet, transform.Find("Bullet"), "LoadBullet()");
        this.LoadComponent(ref this.entity, transform.Find("Entity"), "LoadEntity()");
    }
}
