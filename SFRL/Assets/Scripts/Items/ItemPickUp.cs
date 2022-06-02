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
		
		if (other.tag.Equals("Player") && isDone == false)
		{
			item.PickupHealth(other.GetComponent<PlayerDamageHandler>());
			item.PickupWeapon(other.GetComponent<Shooting>());
			item.PickupCharacter(other.GetComponent<Norman>());
			item.PickupMovement(other.GetComponent<PlayerMovement>());
			isDone = true;
			Debug.Log("Applied");
			Destroy(gameObject);
		}
	}
}
