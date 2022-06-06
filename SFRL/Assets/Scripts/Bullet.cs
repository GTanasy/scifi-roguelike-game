using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class Bullet : MonoBehaviour
{
    public bool _isPlayerBullet;
    public float _damage;

    public bool isCriticalHit;        

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerDamageHandler player = hitInfo.GetComponent<PlayerDamageHandler>();
        EnemyDamageHandler enemy = hitInfo.GetComponent<EnemyDamageHandler>();
        NormanGunShield normanGunShield = hitInfo.GetComponent<NormanGunShield>();
        NormanGrenade normanGrenade = hitInfo.GetComponent<NormanGrenade>();
        Bullet bullet = hitInfo.GetComponent<Bullet>();
        NormanPiercingShot normanPiercing = hitInfo.GetComponent<NormanPiercingShot>();
        BuyableDoor door = hitInfo.GetComponentInParent<BuyableDoor>();
       
        if (player != null)
        {
            player.TakeDamage(_damage);
        }
        else if (enemy != null && _isPlayerBullet)
        {
            enemy.TakeDamage(_damage);           
            DamagePopup.Create(enemy.transform.position, _damage, isCriticalHit, enemy._hasShield);           
        }        
        if (bullet != null || normanPiercing != null)
        {
            return;
        }
        if (enemy != null && !_isPlayerBullet)
        {
            return;
        }
        if (_isPlayerBullet && normanGunShield != null)
        {
            return;
        }
        if (normanGrenade != null)
        {            
            return;
        }
        if (door != null)
        {
            return;
        }
        Destroy(gameObject);
    }
}
