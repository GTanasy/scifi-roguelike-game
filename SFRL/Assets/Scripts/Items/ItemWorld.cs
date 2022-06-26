using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    Item item;
    ItemPickUp _itemPickUp;

    private void Awake()
    {
        _itemPickUp = GetComponent<ItemPickUp>();
    }

    void Start()
    {
        item = new Item { playerPassive = _itemPickUp.item, amount = 1 };
    }
    public void SetItem(Item item)
    {
        this.item = item;
    }

    public Item GetItem()
    {
        return item;
    }
}
