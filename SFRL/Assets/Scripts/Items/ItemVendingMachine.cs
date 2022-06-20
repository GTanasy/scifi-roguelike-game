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
    TMP_Text buyText;

    public string vendingType;

    void Start()
    {
        buyText = GameObject.Find("UI/PopUpPrice").GetComponent<TextMeshProUGUI>();       
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
            buyText.text = "Press (" + GetComponentInChildren<Interactable>().interactKey.ToString()
            + ") to buy a random health item Cost: (" + price + ")";
        }
        else if (vendingType == "Weapon")
        {
            buyText.text = "Press (" + GetComponentInChildren<Interactable>().interactKey.ToString()
            + ") to buy a random weapon item Cost: (" + price + ")";
        }
        else if (vendingType == "Movement")
        {
            buyText.text = "Press (" + GetComponentInChildren<Interactable>().interactKey.ToString()
            + ") to buy a random movement item Cost: (" + price + ")";
        }
        else if (vendingType == "Hero")
        {
            buyText.text = "OUT OF ORDER";
        }
        else
        {
            buyText.text = "ERROR no vending machine type";
        }        
    }

    public void DenotifyPlayer()
    {
        buyText.text = "";
    }
}
