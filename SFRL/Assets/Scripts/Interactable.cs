using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    public UnityEvent interactAction;

    // Update is called once per frame
    void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(interactKey))
            {
                interactAction.Invoke();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = true;
            var door = GetComponentInParent<BuyableDoor>();
            var vending = GetComponentInParent<ItemVendingMachine>();
            if (door != null)
            {
            door.NotifyPlayer();
            }
            else if (vending != null)
            {
                vending.NotifyPlayer();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInRange = false;
            var door = GetComponentInParent<BuyableDoor>();
            var vending = GetComponentInParent<ItemVendingMachine>();
            if (door != null)
            {
                door.DenotifyPlayer();
            }
            else if (vending != null)
            {
                vending.DenotifyPlayer();
            }
        }
    }
}
