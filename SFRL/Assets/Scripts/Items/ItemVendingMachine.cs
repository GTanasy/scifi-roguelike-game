using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemVendingMachine : MonoBehaviour
{
    public int price;
    public int priceMultiplier;
    public GameObject itemSpawner;
    public Transform iSP;
    public int itemsBought;
    TMP_Text _buyText;

    public string vendingType;

    void Start()
    {
        _buyText = GameObject.Find("UI/PopUpPrice").GetComponent<TextMeshProUGUI>();       
    }

    private void Update()
    {
    }
    public void BuyItem()
    {
        Debug.Log("Price: " + price);
        Debug.Log("Items Bought: " + itemsBought);
        if (MoneyController.moneyController.credits >= price && vendingType != "Hero")
        {
            Instantiate(itemSpawner, iSP.position, iSP.rotation);
            MoneyController.moneyController.credits -= price;
            itemsBought++;
            price += (itemsBought * priceMultiplier);
            NotifyPlayer();
        }
    }

    public void NotifyPlayer()
    {
        if (vendingType == "Health")
        {
            _buyText.text = "Press (" + GetComponentInChildren<Interactable>().interactKey.ToString()
            + ") to buy a random health item Cost: (" + price + ")";
        }
        else if (vendingType == "Weapon")
        {
            _buyText.text = "Press (" + GetComponentInChildren<Interactable>().interactKey.ToString()
            + ") to buy a random weapon item Cost: (" + price + ")";
        }
        else if (vendingType == "Movement")
        {
            _buyText.text = "Press (" + GetComponentInChildren<Interactable>().interactKey.ToString()
            + ") to buy a random movement item Cost: (" + price + ")";
        }
        else if (vendingType == "Hero")
        {
            _buyText.text = "OUT OF ORDER";
        }
        else
        {
            _buyText.text = "ERROR no vending machine type";
        }        
    }

    public void DenotifyPlayer()
    {
        _buyText.text = "";
    }
}
