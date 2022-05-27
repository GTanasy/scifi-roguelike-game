using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class Bullet : MonoBehaviour
{
    public string _bulletType;
    public BasicCharacter _bulletDamage;
    float _damage;
    void Start()
    {
        _damage = _bulletDamage.damage;
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerDamageHandler player = hitInfo.GetComponent<PlayerDamageHandler>();
        EnemyDamageHandler enemy = hitInfo.GetComponent<EnemyDamageHandler>();
        NormanGunShield normanGunShield = hitInfo.GetComponent<NormanGunShield>();
        NormanGrenade normanGrenade = hitInfo.GetComponent<NormanGrenade>();
        Bullet bullet = hitInfo.GetComponent<Bullet>();
        NormanPiercingShot normanPiercing = hitInfo.GetComponent<NormanPiercingShot>();
       
        if (player != null)
        {
            player.TakeDamage(20);
        }
        if (enemy != null && _bulletType.Equals("Norman"))
        {
            enemy.TakeDamage(_damage);          
        }
        if (bullet != null || normanPiercing != null)
        {
            return;
        }
        if (enemy != null && _bulletType.Equals("notNorman"))
        {
            return;
        }
        if (_bulletType.Equals("Norman") && normanGunShield != null)
        {
            return;
        }
        if (normanGrenade != null)
        {
            return;
        }        
        Destroy(gameObject);
    }
}
