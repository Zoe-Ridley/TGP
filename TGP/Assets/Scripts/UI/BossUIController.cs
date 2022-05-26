using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIController : MonoBehaviour
{
    private EnemyAI m_enemyAI;

    [Header("Slider")]
    public Slider slider;
    public Image fill;

    void Start()
    {
        slider = slider.GetComponent<Slider>();
        fill = slider.GetComponent<Image>();

        m_enemyAI.m_health = 100;
        slider.value = m_enemyAI.m_health;
    }

    void Update()
    {
        slider.value = m_enemyAI.m_health;
        slider.maxValue = m_enemyAI.m_health;
    }
}
