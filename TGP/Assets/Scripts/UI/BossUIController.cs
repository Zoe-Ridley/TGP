using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIController : MonoBehaviour
{
    [SerializeField] private GameObject m_boss;
    private BossAI m_bossAI;

    [Header("Slider")]
    public Slider slider;
    public Image fill;

    void Start()
    {
        m_bossAI = m_boss.GetComponent<BossAI>();

        slider = slider.GetComponent<Slider>();
        fill = slider.GetComponent<Image>();

        slider.value = m_bossAI.m_health;
    }

    void Update()
    {
        slider.value = m_bossAI.m_health;
        slider.maxValue = m_bossAI.m_health;
    }
}
