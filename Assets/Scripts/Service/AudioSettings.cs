using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private Image _sfxImage, _musicImage;
    [SerializeField] private Sprite _sfxOn, _sfxOff, _musicOn, _musicOff;
    [SerializeField] private ButtonBase _sfxButton, _musicButton;

    private const float On = 0;
    private const float Off = -80;

    private readonly string SFXSave = "SFXVolume";
    private readonly string SFX = "SFX";
    private readonly string MusicSave = "MusicVolume";
    private readonly string Music = "Music";

    private bool _sfxValue;
    private bool _musicValue;
    public bool SFXValue { get => _sfxValue; set { _sfxValue = value; ToggleSFX(value); } }
    public bool MusicValue { get => _musicValue; set { _musicValue = value; ToggleMusic(value); } }

    private void Start()
    {
        //_sfxButton.OnClick.AddListener(() => { SFXValue = !SFXValue; });
        _musicButton.OnClick.AddListener(() => { MusicValue = !MusicValue; });

        //SFXValue = PlayerPrefs.GetInt(SFXSave, 1) == 1;
        MusicValue = PlayerPrefs.GetInt(MusicSave, 1) == 1;
    }

    private void ToggleSFX(bool value)
    {
        _sfxImage.sprite = value ? _sfxOn : _sfxOff;
        _mixer.audioMixer.SetFloat(SFX, value ? On : Off);
        PlayerPrefs.SetInt(SFXSave, value ? 1 : 0);
    }

    private void ToggleMusic(bool value)
    {
        _musicImage.sprite = value ? _musicOn : _musicOff;
        _mixer.audioMixer.SetFloat(Music, value ? On : Off);
        PlayerPrefs.SetInt(MusicSave, value ? 1 : 0);
    }
}