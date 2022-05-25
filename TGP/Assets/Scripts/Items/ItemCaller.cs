using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCaller : MonoBehaviour
{
    private Item m_ItemData;

    public void Init(Item itemData)
    {
        m_ItemData = itemData;
    }
}
