using UnityEngine;

public class UIManager : HuySingleton<UIManager>
{
    [Header("===UI Manager===")]
    [SerializeField] private SettingsUIManager settings;
    [SerializeField] private MainUIManager main;

    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.settings, transform.Find("Settings"), "LoadSettings");
    }

    public void DisplaySettings()
    {
        this.settings.gameObject.SetActive(true);
        this.main.gameObject.SetActive(false);
    }
    
    public void DisplayMain()
    {
        this.main.gameObject.SetActive(true);
        this.settings.gameObject.SetActive(false);
    }

    public void CloseAll()
    {
        this.settings.gameObject.SetActive(false);
        this.main.gameObject.SetActive(false);
    }
}
