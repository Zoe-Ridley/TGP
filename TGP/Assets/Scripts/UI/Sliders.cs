using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sliders : MonoBehaviour
{
    static float m_musicBarValue = 0.5f;
    static float m_soundBarValue = 0.5f;
    [SerializeField] private Slider m_musicBar; 
    [SerializeField] private Slider m_soundBar;

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
}
