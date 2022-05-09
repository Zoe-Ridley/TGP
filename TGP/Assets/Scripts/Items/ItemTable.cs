using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemTable : ScriptableObject
{
    [System.Serializable]
    public class Drop 
    {
        public Item m_Drop;
        public int m_Weight;
    }

    public List<Drop> table;

    [HideInInspector]
    int m_totalWeight = -1;

    public int TotalWeight
    {
        get
        {
            if(m_totalWeight == -1)
            {
                GetTotalWeight();
            }
            return m_totalWeight;
        }
    }

    void GetTotalWeight()
    {
        m_totalWeight = 0;
        for(int i = 0; i < table.Count; i++)
        {
            m_totalWeight += table[i].m_Weight;
        }
    }

    public Item GetLoot()
    {
        int value = UnityEngine.Random.Range(0, 100);

        for (int i = 0; i < table.Count; i++)
        {
            value -= table[i].m_Weight;

            if(value < 0)
            {
                return table[i].m_Drop;
            }
        }
        return table[0].m_Drop;
    }
}
