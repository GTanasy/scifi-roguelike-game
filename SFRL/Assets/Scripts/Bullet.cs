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
       
        if (norman != null)
        {
            norman.TakeDamage(20);
            Destroy(gameObject);
        }
        if (testEnemy != null)
        {
            testEnemy.TakeDamage(20);
            Destroy(gameObject);
        }
        if (_bulletType.Equals("notNorman") && normanGunShield != null)
        {
            Destroy(gameObject);
        }                
    }
}
