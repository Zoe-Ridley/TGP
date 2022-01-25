using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDamage : MonoBehaviour
{
    [SerializeField] private GameObject m_weaponPrefab;

    public GameObject m_player = null;
    public GameObject m_enemy = null;

    private CharacterController m_playerController;
    private Animator m_playerAnimator;

    private void Awake()
    {
        m_player = GetComponent<GameObject>();
        m_enemy = GetComponent<GameObject>();
    }

    private void Start()
    {
        m_playerController = GetComponent<CharacterController>();
        m_playerAnimator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log("hit");
            //Destroy(other.gameObject);
            UIsystem.instance.MeleeDamage(m_enemy);
        }
    }
}
