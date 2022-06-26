using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    UI_Inventory _uiInventory;

    Inventory _inventory;

    void Awake()
    {
        _uiInventory = GameObject.Find("GameHandler/UI/Canvas/TabMenu/ItemInventory").GetComponent<UI_Inventory>();
    }
    void Start()
    {
        _inventory = new Inventory();
        _uiInventory.SetInventory(_inventory);
    }

    public Inventory GetPlayerInventory()
    {
        return _inventory;
    }
}
