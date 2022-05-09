using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;

    private Rigidbody2D m_RB;
    private Vector2 m_playerDir;
    private SpriteRenderer m_spriteRenderer;

    private CharacterController m_playerController;
    public Animator m_playerAnimator;

    void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();

        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_playerController = GetComponent<CharacterController>();
        m_playerAnimator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        m_playerAnimator.SetFloat("Horizontal", movement.x);
        m_playerAnimator.SetFloat("Vertical", movement.y);
        m_playerAnimator.SetFloat("Magnitude", movement.magnitude);

        transform.position = transform.position + movement * Time.deltaTime;

        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");

        m_playerDir = new Vector2(dirX, dirY).normalized;
    }

    void FixedUpdate()
    {
        m_RB.velocity = new Vector2(m_playerDir.x * playerSpeed, m_playerDir.y * playerSpeed);
    }
}
