using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public PlayerPassive playerPassive;
    public int amount = 1;

    public Sprite GetSprite()
    {
        return playerPassive.icon;
    }

    public string GetDescription()
    {
        return playerPassive.description;
    }
}
