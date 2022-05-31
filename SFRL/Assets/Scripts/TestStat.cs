using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;

public class TestStat : MonoBehaviour
{
	public float qCD;
	public float Duration;

	protected void Apply(Norman player)
	{
		player._maxGunShieldCooldown.AddModifier(new StatModifier(qCD, StatModType.Flat, this));		

		//player.StartCoroutine(Timer(Duration, player));
	}

	protected void OnTriggerEnter2D(Collider2D other)
	{
		Norman player = other.GetComponent<Norman>();
		if (player != null)
		{			
			Apply(player);			
			Debug.Log("Applied");
			gameObject.SetActive(false);
		}
	}

	private IEnumerator Timer(float duration, PlayerDamageHandler player)
	{
		yield return new WaitForSeconds(duration);
		player._maxShield.RemoveAllModifiersFromSource(this);
	}
}
