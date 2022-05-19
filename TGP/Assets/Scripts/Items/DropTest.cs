using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTest : MonoBehaviour
{
    [SerializeField] ItemTable lootTable;

    [SerializeField]
    private GameObject[] PowerUp;

    public Transform m_SpawnPoint;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Item item = lootTable.GetLoot();
            Debug.Log(item.name);
            for(int i = 0; i < PowerUp.Length; i++)
            {
                if (PowerUp[i].name == item.name)
                {
                    Instantiate(PowerUp[i], m_SpawnPoint.position, Quaternion.identity);
                    Debug.Log(PowerUp[i].name);
                    break;
                }
            }
        }
    }
}
