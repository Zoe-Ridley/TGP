using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Bullet variables")]
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private float m_firingForce;
    [SerializeField] private float m_coolDownTime;
    private float m_coolDown;

    void Update()
    {

        m_coolDown -= Time.deltaTime;
        //Debug.Log(m_coolDown);
        if (Input.GetMouseButtonDown(1) && m_coolDown < 0f)
        {
            //All this works and fires a bullet prefab towards the mouse location
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector2 toMouse = mousePos - (Vector2)transform.position;
            GameObject tempRef = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, toMouse.normalized)));
            tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_firingForce, ForceMode2D.Impulse);
            m_coolDown = m_coolDownTime;
        }
    }
}
