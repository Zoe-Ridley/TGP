using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTest : MonoBehaviour
{
    [SerializeField] ItemTable lootTable;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Item item = lootTable.GetLoot();
            Debug.Log(item.name);
        }
    }
}
