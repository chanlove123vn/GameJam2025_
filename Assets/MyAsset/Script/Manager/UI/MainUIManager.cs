using UnityEngine;
using UnityEngine.UI;

public class MainUIManager : HuySingleton<MainUIManager>
{
    //==========================================Variable==========================================
    [Header("===Main UI Manager===")]
    [SerializeField] private UIManager ui;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button settingsBtn;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref ui, transform.parent, "LoadUI");
        this.LoadComponent(ref this.resumeBtn, transform.Find("ResumeBtn"), "LoadResumeBtn");
        this.LoadComponent(ref this.settingsBtn, transform.Find("SettingsBtn"), "LoadSettingsBtn");
    }

    private void Start()
    {
        this.resumeBtn.onClick.AddListener(this.ClickResumeBtn);
        this.settingsBtn.onClick.AddListener(this.ClickSettingsBtn);
    }

    //===========================================Method===========================================
    private void ClickResumeBtn()
    {
        this.ui.CloseAll();
    }

    private void ClickSettingsBtn()
    {
        this.ui.DisplaySettings();
    }
}
