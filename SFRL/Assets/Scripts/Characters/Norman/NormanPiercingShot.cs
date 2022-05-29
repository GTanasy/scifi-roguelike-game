using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class NormanPiercingShot : MonoBehaviour
{
    public int _damage;
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Norman norman = hitInfo.GetComponent<Norman>();
        EnemyDamageHandler enemy = hitInfo.GetComponent<EnemyDamageHandler>();
        NormanGunShield normanGunShield = hitInfo.GetComponent<NormanGunShield>();
        Bullet bullet = hitInfo.GetComponent<Bullet>();

        if (enemy != null)
        {
            enemy.TakeDamage(_damage);
            DamagePopup.Create(enemy.transform.position, _damage, false, enemy._hasShield);
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
