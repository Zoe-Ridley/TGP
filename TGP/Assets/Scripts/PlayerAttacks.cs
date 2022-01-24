using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private GameObject m_weaponPrefab;

    public GameObject m_player = null;
    public GameObject m_enemy = null;

    public Transform m_followPlayer;
    public Vector3 m_offset;

    private CharacterController m_playerController;
    private Animator m_playerAnimator;

    private void Start()
    {
        m_playerController = GetComponent<CharacterController>();
        m_playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        transform.position = m_followPlayer.position + m_offset;

        if (Input.GetMouseButtonDown(0))
        {
            m_playerAnimator.SetTrigger("melee");
            GameObject tempRef = Instantiate<GameObject>(m_weaponPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
}
