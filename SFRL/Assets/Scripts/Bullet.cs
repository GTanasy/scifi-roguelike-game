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
        Norman norman = hitInfo.GetComponent<Norman>();
        TestEnemy testEnemy = hitInfo.GetComponent<TestEnemy>();
        NormanGunShield normanGunShield = hitInfo.GetComponent<NormanGunShield>();
        NormanGrenade normanGrenade = hitInfo.GetComponent<NormanGrenade>();
        Bullet bullet = hitInfo.GetComponent<Bullet>();
        NormanPiercingShot normanPiercing = hitInfo.GetComponent<NormanPiercingShot>();
       
        if (norman != null)
        {
            norman.TakeDamage(20);
        }
        if (testEnemy != null && _bulletType.Equals("Norman"))
        {
            testEnemy.TakeDamage(_damage);          
        }
        if (bullet != null || normanPiercing != null)
        {
            return;
        }
        if (testEnemy != null && _bulletType.Equals("notNorman"))
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
