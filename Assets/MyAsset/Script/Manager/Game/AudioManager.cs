using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : HuySingleton<AudioManager>
{
    //==========================================Variable==========================================
    [Header("===Audio Manager===")]
    [SerializeField] private AudioMixer mainMixer;

    private const string MASTER_PARAM = "MasterVolume";
    private const string MUSIC_PARAM = "MusicVolume";
    private const string SOUNDPARAM = "SoundVolume";

    //===========================================Unity============================================
    private void Start()
    {
        LoadVolumes();
    }

    //===========================================Method===========================================
    public void SetMasterVolume(float value)
    {
        SetVolume(MASTER_PARAM, value);
        //PlayerPrefs.SetFloat(MASTER_PARAM, value);
    }

    public void SetMusicVolume(float value)
    {
        SetVolume(MUSIC_PARAM, value);
        //PlayerPrefs.SetFloat(MUSIC_PARAM, value);
    }

    public void SetSoundVolume(float value)
    {
        SetVolume(SOUNDPARAM, value);
        //PlayerPrefs.SetFloat(SFX_PARAM, value);
    }

    private void SetVolume(string parameter, float value)
    {
        // From slider value (0-1) to dB (-80 to 0)
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat(parameter, dB);
    }

    public void LoadVolumes()
    {
        float master = PlayerPrefs.GetFloat(MASTER_PARAM, 1f);
        float music = PlayerPrefs.GetFloat(MUSIC_PARAM, 1f);
        float sfx = PlayerPrefs.GetFloat(SOUNDPARAM, 1f);

        SetVolume(MASTER_PARAM, master);
        SetVolume(MUSIC_PARAM, music);
        SetVolume(SOUNDPARAM, sfx);
    }
}
