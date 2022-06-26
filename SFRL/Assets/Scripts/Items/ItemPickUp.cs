using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using TMPro;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
	public PlayerPassive item;
	bool _isDone = false;

	ItemWorld _itemWorld;
	Inventory _inventory;

	Image _icon;
	RectTransform _iconScale;
	TMP_Text _titleText;
	TMP_Text _descriptionText;
    void Awake()
    {
		_itemWorld = GetComponent<ItemWorld>();
		_icon = GameObject.Find("GameHandler/UI/Canvas/ItemPopUp/ItemIcon").GetComponent<Image>();
		_titleText = GameObject.Find("GameHandler/UI/Canvas/ItemPopUp/ItemName").GetComponent<TextMeshProUGUI>();
		_descriptionText = GameObject.Find("GameHandler/UI/Canvas/ItemPopUp/ItemDescription").GetComponent<TextMeshProUGUI>();
		_iconScale = _icon.GetComponent<RectTransform>();
		_iconScale.localScale = Vector3.zero;		
	}

    protected void OnTriggerEnter2D(Collider2D other)
	{
		PlayerDamageHandler player = other.GetComponent<PlayerDamageHandler>();
		PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
		if (other.tag.Equals("Player") && _isDone == false && player != null)
		{
			GetComponent<AudioSource>().Play();
			item.PickupHealth(other.GetComponent<PlayerDamageHandler>());
			item.PickupWeapon(other.GetComponent<Shooting>());
			item.PickupCharacter(other.GetComponent<Norman>());
			item.PickupMovement(other.GetComponent<PlayerMovement>());
			player.RefreshHealthBars();

			_inventory = playerInventory.GetPlayerInventory();
			_inventory.AddItem(_itemWorld.GetItem());
			
			_isDone = true;
			GetComponent<SpriteRenderer>().enabled = false;
			StartCoroutine(PickUpText());
		}
	}

	IEnumerator PickUpText()
    {
		_iconScale.localScale = Vector3.one;
		_icon.sprite = item.icon;
		_titleText.text = item.name;
		_descriptionText.text = item.description;
		yield return new WaitForSeconds(2);
		_iconScale.localScale = Vector3.zero;
		_titleText.text = "";
		_descriptionText.text = "";
		Destroy(gameObject);
    }
}
