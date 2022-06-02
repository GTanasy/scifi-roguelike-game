using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class EnemyAimWeapon : MonoBehaviour
{
    public BasicEnemy _enemyStats;

    Transform _aimTransform;
    Transform _player;

    public string _enemyType;

    public SpriteRenderer _gun;
    public Camera _cam;

    public Transform _firePoint;

    public GameObject _bulletPrefab;

    public float _bulletForce = 20f;
    public float _attackRange = 20f;
    float _damage;

    void Awake()
    {
        _aimTransform = transform.Find("Aim");
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        _bulletPrefab = _enemyStats.bulletPrefab;
        _bulletForce = _enemyStats.projectileSpeed;
        _attackRange = _enemyStats.attackRange;
        _damage = _enemyStats.damage;
        _enemyType = _enemyStats.enemyAttackType;
    }

    public void HandleAim()
    {
        if (_player != null)
        {
            Vector3 _lookDirection = (_player.position - transform.position).normalized;
            float accuracy = Random.Range(-30, 30);
            // This gets the angle required to rotate to the player using Atan2 and converts it into degrees
            float _angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 0f;
            _aimTransform.eulerAngles = new Vector3(0, 0, _angle + accuracy);
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
        if (_enemyType.Equals("Shooter"))
        {
            GameObject _bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            _bullet.GetComponent<Bullet>()._damage = _damage;
            _bullet.GetComponent<Bullet>()._isPlayerBullet = false;
            _bullet.GetComponent<Rigidbody2D>().AddForce(_firePoint.right * _bulletForce, ForceMode2D.Impulse);
        }
        else if (_enemyType.Equals("Melee"))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_firePoint.position, _attackRange);

            foreach (Collider2D col in hitEnemies)
            {
                PlayerDamageHandler _player = col.GetComponent<PlayerDamageHandler>();
                if (_player != null)
                {
                    _player.TakeDamage(_damage / 2f);
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
