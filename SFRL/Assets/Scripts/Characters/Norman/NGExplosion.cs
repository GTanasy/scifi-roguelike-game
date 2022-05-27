using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Enemy;

public class NGExplosion : MonoBehaviour
{
    public float _radius = 5.0f;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] _enemyHit = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (Collider2D col in _enemyHit)
        {
            EnemyDamageHandler _enemy = col.GetComponent<EnemyDamageHandler>();
            if (_enemy != null)
            {
                _enemy.ExplosionDamage(80);               
            }
        }
        Invoke("Destroy", 0.5f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
