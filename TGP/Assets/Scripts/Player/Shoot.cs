using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Knife variables")]
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private float m_firingForce;
    [SerializeField] private float m_coolDownTime;
    private float m_coolDown;

    [Header("Reload Variables")]
    [SerializeField] private int m_maxKnivesThrown;
    [SerializeField] private float m_reloadTime;
    private float m_tempReloadTime;
    private int m_currentKnivesThrownLeft;

    private void Start()
    {
        m_tempReloadTime = m_reloadTime;
        m_currentKnivesThrownLeft = m_maxKnivesThrown;
    }

    void Update()
    {

        m_coolDown -= Time.deltaTime;
        //Debug.Log(m_coolDown);
        if ((Input.GetMouseButtonDown(0)) && (m_coolDown < 0f) && (m_currentKnivesThrownLeft != 0)) //Shooting, UI should update with knives left
        {
            //Mouse variables to shoot
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector2 toMouse = mousePos - (Vector2)transform.position;
            GameObject tempRef = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, toMouse.normalized))); //shoot
            tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_firingForce, ForceMode2D.Impulse); //force on knife

            //reload and cooldown application
            m_currentKnivesThrownLeft--;
            m_coolDown = m_coolDownTime;
            Debug.Log(m_currentKnivesThrownLeft);
        }
        else if (m_currentKnivesThrownLeft == 0) ////reloading, UI should say reloading
        {
            //Debug.Log("reloading");
            m_tempReloadTime -= Time.deltaTime;

            if(m_tempReloadTime <= 0f) //Reloaded, UI should update to state knives left
            {
                //Debug.Log("reloaded");
                m_currentKnivesThrownLeft = m_maxKnivesThrown;
                m_tempReloadTime = m_reloadTime;
            }
        }

    }
}
