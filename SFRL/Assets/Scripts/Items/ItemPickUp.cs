using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using TMPro;
using UnityEngine.UI;

public class ItemPickUp : MonoBehaviour
{
	public PlayerPassive item;
	bool isDone = false;

	Image icon;
	RectTransform iconScale;
	TMP_Text titleText;
	TMP_Text descriptionText;
    void Awake()
    {
		icon = GameObject.Find("UI/ItemPopUp/ItemIcon").GetComponent<Image>();
		titleText = GameObject.Find("UI/ItemPopUp/ItemName").GetComponent<TextMeshProUGUI>();
		descriptionText = GameObject.Find("UI/ItemPopUp/ItemDescription").GetComponent<TextMeshProUGUI>();
		iconScale = icon.GetComponent<RectTransform>();
		iconScale.localScale = Vector3.zero;
	}

    protected void OnTriggerEnter2D(Collider2D other)
	{
		PlayerDamageHandler player = other.GetComponent<PlayerDamageHandler>();
		if (other.tag.Equals("Player") && isDone == false && player != null)
		{
			GetComponent<AudioSource>().Play();
			item.PickupHealth(other.GetComponent<PlayerDamageHandler>());
			item.PickupWeapon(other.GetComponent<Shooting>());
			item.PickupCharacter(other.GetComponent<Norman>());
			item.PickupMovement(other.GetComponent<PlayerMovement>());
			player.RefreshHealthBars();
			isDone = true;
			GetComponent<SpriteRenderer>().enabled = false;
			StartCoroutine(PickUpText());
		}
	}

	IEnumerator PickUpText()
    {
		iconScale.localScale = Vector3.one;
		icon.sprite = item.icon;
		titleText.text = item.name;
		descriptionText.text = item.description;
		yield return new WaitForSeconds(2);
		iconScale.localScale = Vector3.zero;
		titleText.text = "";
		descriptionText.text = "";
		Destroy(gameObject);
    }
}
