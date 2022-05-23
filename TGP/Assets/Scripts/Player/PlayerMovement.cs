using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed;

    private Vector2 m_playerDir;
    private SpriteRenderer m_spriteRenderer;

    private Rigidbody2D m_rigidBody;
    public Animator PlayerAnimator;

    private bool facingRight;

    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerAnimator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);

        //Controls which way the player is facing.
        m_spriteRenderer.flipX = !facingRight;

        if (movement.x > 0)
        {
            facingRight = true;
        }

        else if (movement.x < 0)
        {
            facingRight = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (movement.y > 0)
            {
                m_playerAnimator.SetTrigger("meleeBack");
            }
            else if (movement.y < 0)
            {
                m_playerAnimator.SetTrigger("melee");
            }
        }

        PlayerAnimator.SetFloat("Horizontal", movement.x);
        PlayerAnimator.SetFloat("Vertical", movement.y);
        PlayerAnimator.SetFloat("Magnitude", movement.magnitude);

        transform.position = transform.position + movement * Time.deltaTime;

        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");

        m_playerDir = new Vector2(dirX, dirY).normalized;
    }

    void FixedUpdate()
    {
        m_rigidBody.velocity = new Vector2(m_playerDir.x * PlayerSpeed, m_playerDir.y * PlayerSpeed);
    }

}