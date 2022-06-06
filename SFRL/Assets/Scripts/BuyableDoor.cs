using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableDoor : MonoBehaviour
{
    public bool isOpen;
    public int price;
    
    public void OpenDoor()
    {
        if (!isOpen && MoneyController.moneyController.credits >= price)
        {
            isOpen = true;
            MoneyController.moneyController.credits -= price;
            Destroy(gameObject);
        }
    }
}
