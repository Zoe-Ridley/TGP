using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed;
    static float m_playerStaticSpeed;

    private Vector2 m_playerDir;
    private SpriteRenderer m_spriteRenderer;

    private Rigidbody2D m_rigidBody;
    public Animator m_playerAnimator;

    private bool facingRight;

    /*private float m_dashCountUp;
    [SerializeField] private float m_dashCooldown;
    [SerializeField] private float m_dashMultiplier;*/

    void Start()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")) // if the scene is main then set speed back to default
        {
            m_playerStaticSpeed = PlayerSpeed;
        }
        else // else use the speed adjusted by powerups
        {
            PlayerSpeed = m_playerStaticSpeed;
        }
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_playerAnimator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        PlayerSpeed = m_playerStaticSpeed;
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
        //dash system
        /*m_dashCountUp += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift)&& m_dashCountUp >= m_dashCooldown)
        {
            m_rigidBody.AddForce(m_rigidBody.velocity * PlayerSpeed * m_dashMultiplier, ForceMode2D.Impulse);
            m_dashCountUp = 0.0f;
        }

        if (m_rigidBody.velocity.x <= -10 || m_rigidBody.velocity.x >= 10 || m_rigidBody.velocity.y <= -10 || m_rigidBody.velocity.y >= 10)
        {
            m_rigidBody.drag = m_rigidBody.drag * 1.25f;
        }
        else m_rigidBody.drag = 1;*/

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
        m_rigidBody.velocity = new Vector2(m_playerDir.x * PlayerSpeed, m_playerDir.y * PlayerSpeed);
        //m_rigidBody.AddForce(new Vector2(m_playerDir.x * PlayerSpeed, m_playerDir.y * PlayerSpeed), ForceMode2D.Force);
    }

}