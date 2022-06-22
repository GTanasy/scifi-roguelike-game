using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class NormanPiercingShot : MonoBehaviour
{
    public int damage;
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Norman norman = hitInfo.GetComponent<Norman>();
        EnemyDamageHandler enemy = hitInfo.GetComponent<EnemyDamageHandler>();
        NormanGunShield normanGunShield = hitInfo.GetComponent<NormanGunShield>();
        Bullet bullet = hitInfo.GetComponent<Bullet>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            DamagePopup.Create(enemy.transform.position, damage, false, enemy.hasShield);
        }
        if (bullet != null)
        {
            return;
        }
        if (enemy || norman)
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
