using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    //public AudioMixer m_AudioMixer;

    //public void SetMusicVolume (float volume)
    //{
    //    m_AudioMixer.SetFloat("Music", volume);
    //}

    //public void SetSFXVolume(float volume)
    //{
    //    m_AudioMixer.SetFloat("SFX", volume);
    //}

    public static float musicVolume { get; private set; }
    public static float SFXVolume { get; private set; }

    [SerializeField] TextMeshProUGUI MusicText;
    [SerializeField] TextMeshProUGUI SFXText;

    public void OnMusicSliderValueChange(float value)
    {
        musicVolume = value;
        MusicText.text = ((int)(value * 100)).ToString();
        AudioManager.instance.UpdateMixerVolume();
    }

    public void OnSFXSliderValueChange(float value)
    {
        SFXVolume = value;
        SFXText.text = ((int)(value * 100)).ToString();
        AudioManager.instance.UpdateMixerVolume();
    }
}
