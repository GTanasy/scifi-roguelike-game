using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class Bullet : MonoBehaviour
{
    public bool isPlayerBullet;
    public float damage;

    public bool isCriticalHit;        

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        IDamageable damageable = hitInfo.GetComponent<IDamageable>();
        PlayerDamageHandler player = hitInfo.GetComponent<PlayerDamageHandler>();
        EnemyDamageHandler enemy = hitInfo.GetComponent<EnemyDamageHandler>();
        NormanGunShield normanGunShield = hitInfo.GetComponent<NormanGunShield>();
        NormanGrenade normanGrenade = hitInfo.GetComponent<NormanGrenade>();
        Bullet bullet = hitInfo.GetComponent<Bullet>();
        NormanPiercingShot normanPiercing = hitInfo.GetComponent<NormanPiercingShot>();
        BuyableDoor door = hitInfo.GetComponentInParent<BuyableDoor>();
        ItemVendingMachine vending = hitInfo.GetComponentInParent<ItemVendingMachine>();
        ItemPickUp item = hitInfo.GetComponent<ItemPickUp>();
       
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (damageable != null && isPlayerBullet)
        {
            damageable.TakeDamage(damage);           
            DamagePopup.Create(enemy.transform.position, damage, isCriticalHit, enemy.hasShield);
            Destroy(gameObject);
        }
        if (normanGunShield && !isPlayerBullet)
        {
            Destroy(gameObject);
        }
        if (hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        //if (bullet != null || normanPiercing != null)
        //{
        //    return;
        //}
        //if (enemy != null && !isPlayerBullet)
        //{
        //    return;
        //}
        //if (isPlayerBullet && normanGunShield != null)
        //{
        //    return;
        //}
        //if (normanGrenade != null)
        //{            
        //    return;
        //}
        //if (door != null)
        //{
        //    return;
        //}
        //if (vending != null)
        //{
        //    return;
        //}
        //if (item != null)
        //{
        //    return;
        //}
        //Destroy(gameObject);
    }
}
