using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;

[CreateAssetMenu(menuName = "Player Passive Item")]
public class PlayerPassive : BasicItem
{
	[Space]
    [Header("Character Stats Flat")]
	[Space]
	public float HealthBonus;
	public float ShieldBonus;
	public float ShieldRegenBonus;
	public float ArmorBonus;
	public float QCooldownBonus;
	public float ECooldownBonus;

	[Space]
	[Header("Weapon Stats Flat")]
	[Space]
	public int CritChanceBonus;
	public float DamageBonus;
	public float AttackSpeedBonus;
	public float RangeBonus;
	public float ProjectileSpeedBonus;
	public float ReloadSpeedBonus;
	public int MagCapBonus;

	[Space]
	[Header("Movement Stats Flat")]
	[Space]
	public float DashCooldownBonus;
	public float DashLengthBonus;
	public float DashSpeedBonus;
	public float NormalSpeedBonus;

	[Space]
	[Header("Character Stats Percent")]
	[Space]
	public float HealthPercentBonus;
	public float ShieldPercentBonus;
	public float ShieldRegenPercentBonus;
	public float ArmorPercentBonus;
	public float QCooldownPercentBonus;
	public float ECooldownPercentBonus;

	[Space]
	[Header("Weapon Stats Percent")]
	[Space]
	public float CritChancePercentBonus;
	public float DamagePercentBonus;
	public float AttackSpeedPercentBonus;
	public float RangePercentBonus;
	public float ProjectileSpeedPercentBonus;
	public float ReloadSpeedPercentBonus;
	public int MagCapPercentBonus;

	[Space]
	[Header("Movement Stats Percent")]
	[Space]
	public float DashCooldownPercentBonus;
	public float DashLengthPercentBonus;
	public float DashSpeedPercentBonus;
	public float NormalSpeedPercentBonus;

	public void PickupHealth(PlayerDamageHandler player)
	{
		if (HealthBonus != 0)
			player._maxHealth.AddModifier(new StatModifier(HealthBonus, StatModType.Flat, this));
			player.Heal();
		if (ShieldBonus != 0)
			player._maxShield.AddModifier(new StatModifier(ShieldBonus, StatModType.Flat, this));
		if (ShieldRegenBonus != 0)
			player._shieldRegenRate.AddModifier(new StatModifier(ShieldRegenBonus, StatModType.Flat, this));		

		if (HealthPercentBonus != 0)
			player._maxHealth.AddModifier(new StatModifier(HealthPercentBonus, StatModType.PercentMult, this));
		if (ShieldPercentBonus != 0)
			player._maxShield.AddModifier(new StatModifier(ShieldPercentBonus, StatModType.PercentMult, this));
		if (ShieldRegenPercentBonus != 0)
			player._shieldRegenRate.AddModifier(new StatModifier(ShieldRegenPercentBonus, StatModType.PercentMult, this));		
	}

	public void PickupCharacter(Norman player)
    {
		if (QCooldownBonus != 0)
			player._maxGunShieldCooldown.AddModifier(new StatModifier(QCooldownBonus, StatModType.Flat, this));
		if (ECooldownBonus != 0)
			player._maxGrenadeCooldown.AddModifier(new StatModifier(ECooldownBonus, StatModType.Flat, this));

		if (QCooldownPercentBonus != 0)
			player._maxGunShieldCooldown.AddModifier(new StatModifier(QCooldownPercentBonus, StatModType.PercentMult, this));
		if (ECooldownPercentBonus != 0)
			player._maxGrenadeCooldown.AddModifier(new StatModifier(ECooldownPercentBonus, StatModType.PercentMult, this));
	}
	public void PickupWeapon(Shooting player)
	{
		if (CritChanceBonus != 0)
			player._criticalChance.AddModifier(new StatModifier(CritChanceBonus, StatModType.Flat, this));
		if (DamageBonus != 0)
			player._bulletDamage.AddModifier(new StatModifier(DamageBonus, StatModType.Flat, this));
		if (AttackSpeedBonus != 0)
			player._attackSpeed.AddModifier(new StatModifier(AttackSpeedBonus, StatModType.Flat, this));
		if (ProjectileSpeedBonus != 0)
			player._bulletForce.AddModifier(new StatModifier(ProjectileSpeedBonus, StatModType.Flat, this));
		if (ReloadSpeedBonus != 0)
			player._reloadDuration.AddModifier(new StatModifier(ReloadSpeedBonus, StatModType.Flat, this));
		if (MagCapBonus != 0)
			player._maxMagCapacity.AddModifier(new StatModifier(MagCapBonus, StatModType.Flat, this));

		if (CritChancePercentBonus != 0)
			player._criticalChance.AddModifier(new StatModifier(CritChancePercentBonus, StatModType.PercentMult, this));
		if (DamagePercentBonus != 0)
			player._bulletDamage.AddModifier(new StatModifier(DamagePercentBonus, StatModType.PercentMult, this));
		if (AttackSpeedPercentBonus != 0)
			player._attackSpeed.AddModifier(new StatModifier(AttackSpeedPercentBonus, StatModType.PercentMult, this));
		if (ProjectileSpeedPercentBonus != 0)
			player._bulletForce.AddModifier(new StatModifier(ProjectileSpeedPercentBonus, StatModType.PercentMult, this));
		if (ReloadSpeedPercentBonus != 0)
			player._reloadDuration.AddModifier(new StatModifier(ReloadSpeedPercentBonus, StatModType.PercentMult, this));
		if (MagCapPercentBonus != 0)
			player._maxMagCapacity.AddModifier(new StatModifier(MagCapPercentBonus, StatModType.PercentMult, this));
	}
	public void PickupMovement(PlayerMovement player)
	{
		if (DashCooldownBonus != 0)
			player._dashCoolDown.AddModifier(new StatModifier(DashCooldownBonus, StatModType.Flat, this));
		if (DashLengthBonus != 0)
			player._dashLength.AddModifier(new StatModifier(DashLengthBonus, StatModType.Flat, this));
		if (DashSpeedBonus != 0)
			player._dashSpeed.AddModifier(new StatModifier(DashSpeedBonus, StatModType.Flat, this));
		if (NormalSpeedBonus != 0)
			player._normalSpeed.AddModifier(new StatModifier(NormalSpeedBonus, StatModType.Flat, this));

		if (DashCooldownPercentBonus != 0)
			player._dashCoolDown.AddModifier(new StatModifier(DashCooldownPercentBonus, StatModType.PercentMult, this));
		if (DashLengthPercentBonus != 0)
			player._dashLength.AddModifier(new StatModifier(DashLengthPercentBonus, StatModType.PercentMult, this));
		if (DashSpeedPercentBonus != 0)
			player._dashSpeed.AddModifier(new StatModifier(DashSpeedPercentBonus, StatModType.PercentMult, this));
		if (NormalSpeedPercentBonus != 0)
			player._normalSpeed.AddModifier(new StatModifier(NormalSpeedPercentBonus, StatModType.PercentMult, this));
	}

}
