using UnityEngine;
using UnityEngine.UI;

public class SettingsUIManager : HuySingleton<SettingsUIManager>
{
    //==========================================Variable==========================================
    [Header("===Settings UI Manager===")]
    [SerializeField] private UIManager ui;
    [SerializeField] private Slider masterSoundSlider;
    [SerializeField] private Slider musicSoundSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Button backBtn;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.ui, transform.Find("UI"), "LoadUI");
        this.LoadComponent(ref this.masterSoundSlider, transform.Find("MasterSound/Slider"), "LoadMasterSoundSlider");
        this.LoadComponent(ref this.musicSoundSlider, transform.Find("Music/Slider"), "LoadMusicSoundSlider");
        this.LoadComponent(ref this.soundSlider, transform.Find("Sound/Slider"), "LoadSoundSlider");
    }

    private void Update()
    {
        AudioManager.Instance.SetMasterVolume(masterSoundSlider.value);
        AudioManager.Instance.SetMusicVolume(musicSoundSlider.value);
        AudioManager.Instance.SetSoundVolume(soundSlider.value);
    }

    private void Start()
    {
        this.backBtn.onClick.AddListener(ClickBackBtn);
    }

    //===========================================Method===========================================
    private void ClickBackBtn()
    {
        this.ui.DisplayMain();
    }
}
