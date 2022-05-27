using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class Sliders : MonoBehaviour
{
    private static float m_musicBarValue = 0.5f;
    private static float m_soundBarValue = 0.5f;
    [SerializeField] private Slider m_musicBar;
    [SerializeField] private Slider m_soundBar;
    [SerializeField] TextMeshProUGUI MusicText;
    [SerializeField] TextMeshProUGUI SFXText;

    public static float musicVolume { get; private set; }
    public static float SFXVolume { get; private set; }


    private void Start()
    {
        m_musicBar.value = m_musicBarValue;
        m_soundBar.value = m_soundBarValue;
    }
    private void Update()
    {
        m_musicBarValue = m_musicBar.value;
        m_soundBarValue = m_soundBar.value;
    }

    public void OnMusicSliderValueChange()
    {
        musicVolume = m_musicBar.value;
        MusicText.text = ((int)(m_musicBarValue * 100)).ToString();
        AudioManager.instance.UpdateMixerVolume();
    }

    public void OnSFXSliderValueChange()
    {
        SFXVolume = m_soundBar.value;
        SFXText.text = ((int)(m_soundBarValue * 100)).ToString();
        AudioManager.instance.UpdateMixerVolume();
    }
}
