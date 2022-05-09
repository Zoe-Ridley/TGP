using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{

    public GameObject m_player;
    private GameObject m_enemy;

    public Transform m_followPlayer;
    public Vector3 m_offset;

    private Rigidbody m_rigidBody;
    private Animator m_playerAnimator;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            m_playerAnimator.SetTrigger("melee");
            FindObjectOfType<AudioManager>().playAudio("PlayerMelee");
            GameObject knifeClone = Instantiate<GameObject>(m_weaponPrefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(knifeClone, 0.5f);
        }
    }
}
