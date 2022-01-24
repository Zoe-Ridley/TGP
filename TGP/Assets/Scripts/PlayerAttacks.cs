using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    public GameObject m_player = null;
    public GameObject m_enemy = null;

    private CharacterController m_playerController;
    private Animator m_playerAnimator;

    private void Start()
    {
        m_playerController = GetComponent<CharacterController>();
        m_playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_playerAnimator.SetTrigger("Melee");
        }
    }
}
