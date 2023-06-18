using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    // Background
    [SerializeField] private AudioClip _mainMenu;
    [SerializeField] private AudioClip _inGame;

    // Primary
    [SerializeField] private AudioClip _cast;
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip _itemPickup;
    [SerializeField] private AudioClip _unlockDoor;
    [SerializeField] private AudioClip _buttonClick;

    // Secondary
    [SerializeField] private AudioClip _meldEffect;
    [SerializeField] private AudioClip _clairvoyanceEffect;

    // Tertiary
    [SerializeField] private AudioClip _skillBarReplenished;
    [SerializeField] private AudioClip _skillBarLocked;
    [SerializeField] private AudioClip _skillSwitch;

    // Audio Sources
    [SerializeField] private AudioSource _backgroundAudioSource;
    [SerializeField] private AudioSource _primaryAudioSource;
    [SerializeField] private AudioSource _secondaryAudioSource;
    [SerializeField] private AudioSource _tertiaryAudioSource;

    public void PlayPrimarySound (string sound)
    {
        if (_primaryAudioSource.isPlaying) return;

        if (sound == "death") _primaryAudioSource.clip = _death;
        if (sound == "item_pickup") _primaryAudioSource.clip = _itemPickup;
        if (sound == "unlock_door") _primaryAudioSource.clip = _unlockDoor;
        if (sound == "button_click") _primaryAudioSource.clip = _buttonClick;

        _primaryAudioSource.Play();
    }

    public void PlaySecondarySound(string sound)
    {
        if (_secondaryAudioSource.isPlaying) return;

        if (sound == "meldEffect") _secondaryAudioSource.clip = _meldEffect;
        if (sound == "clairvoyanceEffect") _secondaryAudioSource.clip = _clairvoyanceEffect;
        
        _secondaryAudioSource.Play();
    }

    public void PlayTertiarySound(string sound)
    {

        if (sound == "replenished") _tertiaryAudioSource.clip = _skillBarReplenished;
        if (sound == "locked") _tertiaryAudioSource.clip = _skillBarLocked;
        if (sound == "skillSwitch") _tertiaryAudioSource.clip = _skillSwitch;

        _tertiaryAudioSource.Play();
    }

    public void PlayBackgroundMusic(string sound)
    {
        _backgroundAudioSource.Stop();
        if (sound == "mainMenu") _backgroundAudioSource.clip = _mainMenu;
        if (sound == "inGame") _backgroundAudioSource.clip = _inGame;
        _backgroundAudioSource.Play();
    }















    // ---------- Pause

    public void PauseBackgroundMusic() { _backgroundAudioSource.Pause(); }

    public void ResumeBackgroundMusic() { _backgroundAudioSource.Play(); }


    // ---------- Stoppers


    public void StopBackgroundAudio() { if (_backgroundAudioSource.isPlaying) _backgroundAudioSource.Stop(); }
    public void StopPrimaryAudio() { if (_primaryAudioSource.isPlaying) _primaryAudioSource.Stop(); }
    public void StopSecondaryAudio() { if (_secondaryAudioSource.isPlaying) _secondaryAudioSource.Stop(); }
    public void StopTertiaryAudio() { if (_tertiaryAudioSource.isPlaying) _tertiaryAudioSource.Stop(); }

    public void StopAllAudio()
    {
        StopBackgroundAudio();
        StopPrimaryAudio();
        StopSecondaryAudio();
        StopTertiaryAudio();
    }
}
