using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Item m_data;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadItem(Item itemData)
    {
        //Load Current Item sprite
        GameObject m_visuals = Instantiate(itemData.m_ItemGameObject);
        m_visuals.transform.SetParent(this.transform);
        m_visuals.transform.localPosition = Vector3.zero;
        m_visuals.transform.rotation = Quaternion.identity;
    }
}
