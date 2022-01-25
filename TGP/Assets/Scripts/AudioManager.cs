using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Audio[] AudioClip;

    public static AudioManager instance;

    [SerializeField] AudioMixerGroup m_MusicMixerGroup;
    [SerializeField] AudioMixerGroup m_SFXMixerGroup;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Audio a in AudioClip)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.Sound;
            a.source.volume = a.volume;
            a.source.loop = a.loop;

            switch (a.audioType)
            {
                case Audio.AudioTypes.Music:
                    a.source.outputAudioMixerGroup = m_MusicMixerGroup;
                    break;
                case Audio.AudioTypes.SFX:
                    a.source.outputAudioMixerGroup = m_SFXMixerGroup;
                    break;
            }
        }
    }

    private void Start()
    {
        playAudio("Title Theme");
    }

    public void playAudio(string name)
    {
        foreach (Audio a in AudioClip)
        {
            if (a.m_Name == name)
            {
                a.source.Play();
            }
        }
    }

    public void UpdateMixerVolume()
    {
        m_MusicMixerGroup.audioMixer.SetFloat("Music Volume", Mathf.Log10(VolumeSlider.musicVolume) * 20);
        m_SFXMixerGroup.audioMixer.SetFloat("SFX Volume", Mathf.Log10(VolumeSlider.SFXVolume) * 20);
    }
}

