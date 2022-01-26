using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIsystem : MonoBehaviour
{
    public static UIsystem instance { get; private set; }

    public GameObject m_player = null;
    public GameObject m_enemy = null;

    public Text m_playerHealthText;
    public Text m_enemyHealthText;

    int m_playerCurrentHealth = 100;
    int m_enemyCurrentHealth = 100;
    int m_playerMaxXP;

    [SerializeField] private Slider m_playerHealth = null;
    [SerializeField] private Slider m_enemyHealth = null;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        m_player = GetComponent<GameObject>();
        m_enemy = GetComponent<GameObject>();
    }

    public void MeleeDamage(GameObject target)
    {
        if (target == m_enemy)
        {
            m_enemyCurrentHealth -= 15;
            m_enemyHealth.value = m_enemyCurrentHealth;
            m_enemyHealthText.text = m_enemyCurrentHealth.ToString();
        }
    }

    public void AIDamage(GameObject target)
    {
        if (target == m_player)
        {
            m_playerCurrentHealth -= 15;
            m_playerHealth.value = m_playerCurrentHealth;
            m_playerHealthText.text = m_playerCurrentHealth.ToString();
        }
    }
}
