using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyableDoor : MonoBehaviour
{
    public bool isOpen;
    public int price;

    TMP_Text _buyText;

    MoneyController _moneyController;

    void Awake()
    {
        _moneyController = GameObject.Find("GameHandler/MoneyController").GetComponent<MoneyController>();
    }

    void Start()
    {
        _buyText = GameObject.Find("UI/PopUpPrice").GetComponent<TextMeshProUGUI>();
    }
    public void OpenDoor()
    {
        if (!isOpen && _moneyController.credits >= price)
        {
            isOpen = true;
            _moneyController.credits -= price;
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
