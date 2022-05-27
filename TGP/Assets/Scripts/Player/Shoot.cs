using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Shoot : MonoBehaviour
{
    [Header("Knife variables")]
    [SerializeField] private GameObject m_bulletPrefab;
    [SerializeField] private float m_firingForce;
    [SerializeField] private float m_coolDownTime;
    private float m_coolDown;

    [Header("Reload Variables")]
    [SerializeField] private int m_maxKnivesThrown;
    [SerializeField] public float m_reloadTime;
    static float m_reloadTimeStatic;
    private float m_tempReloadTime;
    private int m_currentKnivesThrownLeft;
    private bool m_reloadSoundPlayed;

    [Header("GUI")] 
    [SerializeField] private TextMeshProUGUI m_reloadText;

    private void Start()
    {
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main")) // if the scene is main then set reload time back to default
        {
            m_reloadTimeStatic = m_reloadTime;
        }
        else // else use the time adjusted by powerups
        {
            m_reloadTime = m_reloadTimeStatic;
        }
        m_tempReloadTime = m_reloadTime;
        m_currentKnivesThrownLeft = m_maxKnivesThrown;
    }

    void Update()
    {
        m_reloadTimeStatic = m_reloadTime;
        m_coolDown -= Time.deltaTime;
        //Debug.Log(m_coolDown);

        // sets reload text from the start.
        m_reloadText.SetText(m_currentKnivesThrownLeft + " / " + m_maxKnivesThrown);

        if ((Input.GetMouseButtonDown(0)) && (m_coolDown < 0f) && (m_currentKnivesThrownLeft != 0)) //Shooting, UI should update with knives left
        {
            m_reloadSoundPlayed = false;
            //Mouse variables to shoot
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            Vector2 toMouse = mousePos - (Vector2)transform.position;
            GameObject tempRef = Instantiate<GameObject>(m_bulletPrefab, gameObject.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.Cross(Vector3.forward, toMouse.normalized))); //shoot
            //tempRef.GetComponent<Rigidbody2D>().AddForce(tempRef.transform.right * m_firingForce, ForceMode2D.Impulse); //force on knife

            //reload and cooldown application
            m_currentKnivesThrownLeft--;
            m_coolDown = m_coolDownTime;
            Debug.Log("Knives left:  " +m_currentKnivesThrownLeft);
        }
        else if (m_currentKnivesThrownLeft == 0) ////reloading, UI should say reloading
        {
            //Debug.Log("reloading");
            m_tempReloadTime -= Time.deltaTime;

            if (m_reloadSoundPlayed == false)
            {
                Debug.Log("play sound once");
                FindObjectOfType<AudioManager>().playAudio("Reload Sound");
                m_reloadSoundPlayed = true;
            }
            if(m_tempReloadTime <= 0f) //Reloaded, UI should update to state knives left
            {
                //Debug.Log("reloaded");
                m_currentKnivesThrownLeft = m_maxKnivesThrown;
                m_tempReloadTime = m_reloadTime;
            }
        }

    }
}
