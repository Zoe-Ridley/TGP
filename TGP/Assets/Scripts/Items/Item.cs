using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items")]
public class Item : ScriptableObject
{
    public ItemType m_Type;
    public string Name;
    public GameObject m_ItemGameObject;
    public float m_ChangeNum;
}

public enum ItemType
{
    NONE,
    HEALTH_POTION,
    ATTACK_SPEED,
    MAX_HEALTH,
    MOVE_SPEED
}
