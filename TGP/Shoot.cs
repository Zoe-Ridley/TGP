using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject m_BulletPrefab;
    [SerializeField] private float m_FiringForce;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector2 toMouse = mousePos - (Vector2)transform.position;
            GameObject tempRef = Instantiate<GameObject>(m_BulletPrefab, gameObject.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, toMouse.normalized)));
            tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_FiringForce, ForceMode2D.Impulse);
            //AudioSource.PlayClipAtPoint(m_FireSound, transform.position);

        }
    }
}
