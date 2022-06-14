using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemVendingMachine : MonoBehaviour
{
    public int price;
    public GameObject itemSpawner;
    public Transform iSP;
    int itemsBought;
    TMP_Text buyText;

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
        if (MoneyController.moneyController.credits >= price)
        {
            Instantiate(itemSpawner, iSP.position, iSP.rotation);
            MoneyController.moneyController.credits -= price;
            itemsBought++;
            price += (itemsBought * 50);
            NotifyPlayer();
        }
    }

    public void NotifyPlayer()
    {       
        buyText.text = "Press (" + GetComponentInChildren<Interactable>().interactKey.ToString()
            + ") to buy a random item Cost: (" + price + ")";
    }

    public void DenotifyPlayer()
    {
        buyText.text = "";
    }
}
