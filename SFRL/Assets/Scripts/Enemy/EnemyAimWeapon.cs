using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;

public class EnemyAimWeapon : MonoBehaviour
{
    Transform _aimTransform;
    Transform _player;

    public string enemyType;

    public SpriteRenderer _gun;
    public Camera _cam;

    public Transform _firePoint;

    public GameObject _bulletPrefab;

    public float _bulletForce = 20f;
    public float _attackRange = 20f;

    void Awake()
    {
        _aimTransform = transform.Find("Aim");
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void HandleAim()
    {
        if (_player != null)
        {
            Vector3 _lookDirection = (_player.position - transform.position).normalized;

            // This gets the angle required to rotate the player using Atan2 and converts it into degrees
            float _angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 0f;
            _aimTransform.eulerAngles = new Vector3(0, 0, _angle);
            Vector3 _scale = Vector3.one;

            if (_angle < -90 || _angle > 90)
            {
                _scale.y = -1.0f;
            }
            else
            {
                _scale.y = +1.0f;
            }
            _aimTransform.localScale = _scale;
        }
        
    }

    public void Shoot()
    {
        if (enemyType.Equals("Shooter"))
        {
        GameObject _bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        _bullet.GetComponent<Rigidbody2D>().AddForce(_firePoint.right * _bulletForce, ForceMode2D.Impulse);
        }
        else if (enemyType.Equals("Melee"))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_firePoint.position, _attackRange);

            foreach (Collider2D col in hitEnemies)
            {
                PlayerDamageHandler _player = col.GetComponent<PlayerDamageHandler>();
                if (_player != null)
                {
                    _player.TakeDamage(50);
                }
            }
        }
        else
        {
            Debug.Log("ERROR ENEMY DOES NOT HAVE A TYPE");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_firePoint.position, _attackRange);
    }
}
