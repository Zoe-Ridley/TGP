using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTesting : MonoBehaviour
{
    [SerializeField]
    AudioManager m_AudioManager;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            m_AudioManager.playAudio("Slashing sound");
            Debug.Log("Slash");
        }
    }
}
