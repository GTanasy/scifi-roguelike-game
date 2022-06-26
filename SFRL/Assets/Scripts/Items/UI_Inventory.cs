using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Inventory _inventory;
    Transform _itemSlotContainer;
    Transform _itemSlotTemplate;

    void Awake()
    {
        _itemSlotContainer = transform.Find("ItemSlotContainer");
        _itemSlotTemplate = _itemSlotContainer.Find("itemSlotTemplate");
    }

    public void SetInventory(Inventory inventory)
    {
        _inventory = inventory;

        _inventory.OnItemListChanged += Inventory_OnItemListChanged;

        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    void RefreshInventoryItems()
    {
        foreach (Transform child in _itemSlotContainer)
        {
            if (child == _itemSlotTemplate)
            {
                continue;
            }
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 100f;
        foreach (Item item in _inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(_itemSlotTemplate, _itemSlotContainer).GetComponent<RectTransform>();
            TooltipInfomation tooltipInfo = itemSlotRectTransform.GetComponent<TooltipInfomation>();
            tooltipInfo.playerPassive = item.playerPassive;
            itemSlotRectTransform.gameObject.SetActive(true);        
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TMP_Text amountText = itemSlotRectTransform.Find("amountText").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                amountText.text = item.amount.ToString();
            }
            else
            {
                amountText.text = "";
            }
            x++;
            if (x > 11)
            {
                x = 0;
                y--;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Cursor Entering " + gameObject.GetComponentInChildren<Image>().sprite.name + " GameObject");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Cursor Exiting " + name + " GameObject");
    }
}
