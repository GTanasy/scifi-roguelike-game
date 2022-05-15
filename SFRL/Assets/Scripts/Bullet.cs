using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Norman norman = hitInfo.GetComponent<Norman>();
        TestEnemy testEnemy = hitInfo.GetComponent<TestEnemy>();

        if (norman != null)
        {
            norman.TakeDamage(20);
        }
        if (testEnemy != null)
        {
            testEnemy.TakeDamage(20);
        }
        Destroy(gameObject);
    }
}
