using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Create New Item")]
public class BasicItem : ScriptableObject
{
    public enum itemType
    {
        PlayerPassive,
        EnemyPassive,
        PassiveAction,
        Boss,
        Hero,
    };

    public enum rarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
        Exotic
    };

    [Space(15)]
    [Header("Item Settings")]

    public itemType type;
    public new string name;
    [TextArea(5,10)]
    public string description;
    public int iD;
    public Sprite icon; 
    public rarity rarityType;        
}
