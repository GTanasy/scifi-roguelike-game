using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class Bullet : MonoBehaviour
{
    public string _bulletType;
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Norman norman = hitInfo.GetComponent<Norman>();
        TestEnemy testEnemy = hitInfo.GetComponent<TestEnemy>();
        NormanGunShield normanGunShield = hitInfo.GetComponent<NormanGunShield>();
        NormanGrenade normanGrenade = hitInfo.GetComponent<NormanGrenade>();        
       
        if (norman != null)
        {
            norman.TakeDamage(20);
        }
        if (testEnemy != null && _bulletType.Equals("Norman"))
        {
            testEnemy.TakeDamage(20);          
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
