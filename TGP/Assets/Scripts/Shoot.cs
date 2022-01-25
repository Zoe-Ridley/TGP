using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private float m_FiringForce;
    [SerializeField] private float m_angleOfFire = 20f;
    [SerializeField] private float m_coolDownTime;
    private float m_coolDown;

    private Rigidbody2D m_RB;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Horizontal"))
        {
            m_RB.AddForce(Vector2.right * -10f, ForceMode2D.Force);
        }

        m_coolDown -= Time.deltaTime;
        //Debug.Log(m_coolDown);
        if (Input.GetMouseButtonDown(0) && m_coolDown < 0f)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector2 toMouse = mousePos - (Vector2)transform.position;
            //Debug.Log(Vector2.SignedAngle(Vector2.up, mousePos));
            GameObject tempRef = Instantiate<GameObject>(m_BulletPrefab, gameObject.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, toMouse.normalized)));
            tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_FiringForce, ForceMode2D.Impulse);
            m_coolDown = m_coolDownTime;
        }
    }
}
