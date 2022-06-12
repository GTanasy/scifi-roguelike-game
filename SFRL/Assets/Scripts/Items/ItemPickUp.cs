using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;

public class ItemPickUp : MonoBehaviour
{
	public PlayerPassive item;
	bool isDone = false;   

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
			Destroy(gameObject, GetComponent<AudioSource>().clip.length);
		}
	}
}
