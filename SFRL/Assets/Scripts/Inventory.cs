using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory 
{
    public event EventHandler OnItemListChanged;
    List<Item> _itemList;

    public Inventory()
    {
        _itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        bool itemAlreadyInInventory = false;
        foreach (Item inventoryItem in _itemList)
        {
            if (inventoryItem.playerPassive == item.playerPassive)
            {
                inventoryItem.amount += item.amount;
                itemAlreadyInInventory = true;
            }
        }
        if (!itemAlreadyInInventory)
        {
            _itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return _itemList;
    }
}
