using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    public static Action onClick;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _onClickClip;
    [SerializeField] private AudioClip _startSpinClip;
    [SerializeField] private AudioClip _winClip;

    private void Start()
    {
        onClick += OnClick;
        Instance = this;
    }

    private void OnClick()
    {
        _audioSource.clip = _onClickClip;
        _audioSource.Play();
    }

    public void StartSpin()
    {
        Instance._audioSource.PlayOneShot(Instance._startSpinClip);
    }

    public void Win()
    {
        Instance._audioSource.PlayOneShot(Instance._winClip);
    }
}