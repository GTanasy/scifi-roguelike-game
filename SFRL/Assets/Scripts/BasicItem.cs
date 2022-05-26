using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item")]
public class BasicItem : ScriptableObject
{
    public enum itemType
    {
        passive,
        hero,
    };

    [Space(15)]
    [Header("Item Settings")]

    public new string name;
    public string description;
    public int iD;
    
}
