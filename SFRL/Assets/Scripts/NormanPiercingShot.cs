using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class NormanPiercingShot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Norman norman = hitInfo.GetComponent<Norman>();
        TestEnemy testEnemy = hitInfo.GetComponent<TestEnemy>();
        NormanGunShield normanGunShield = hitInfo.GetComponent<NormanGunShield>();
        if (testEnemy != null)
        {
            testEnemy.TakeDamage(80);
        }
        if (testEnemy || norman)
        {
            return;
        }
        if (normanGunShield != null)
        {
            return;
        }
        Destroy(gameObject);
    }
}
