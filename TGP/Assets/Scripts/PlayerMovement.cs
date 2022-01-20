using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed;

    private Rigidbody2D m_RB;
    private Vector2 m_playerDir;
    private SpriteRenderer m_spriteRenderer;

    //create a bool to check if player is facing right or left, if they're already facing that direction don't flip them.
    private bool m_facing;

    void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();

        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float dirX = Input.GetAxisRaw("Horizontal");
        float dirY = Input.GetAxisRaw("Vertical");

        m_playerDir = new Vector2(dirX, dirY).normalized;
    }

    void FixedUpdate()
    {
        m_RB.velocity = new Vector2(m_playerDir.x * playerSpeed, m_playerDir.y * playerSpeed);
    }
}
