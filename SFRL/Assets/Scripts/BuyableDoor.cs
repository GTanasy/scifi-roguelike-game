using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyableDoor : MonoBehaviour
{
    public bool isOpen;
    public int price;

    TMP_Text _buyText;

    void Start()
    {
        _buyText = GameObject.Find("UI/PopUpPrice").GetComponent<TextMeshProUGUI>();
    }
    public void OpenDoor()
    {
        if (!isOpen && MoneyController.moneyController.credits >= price)
        {
            isOpen = true;
            MoneyController.moneyController.credits -= price;
            Destroy(gameObject);
        }
    }

    public void NotifyPlayer()
    {
        _buyText.text = "Press (" + GetComponentInChildren<Interactable>().interactKey.ToString() + ") to open door Cost: (" + price + ")";
    }

    public void DenotifyPlayer()
    {
        _buyText.text = "";
    }
}
