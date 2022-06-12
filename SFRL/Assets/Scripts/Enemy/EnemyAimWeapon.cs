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

    public Transform[] _firePoint;

    public GameObject _bulletPrefab;

    public CharacterStat _bulletForce;
    public CharacterStat _attackRange;
    public CharacterStat _damage;
    public CharacterStat _spread;

    void Awake()
    {
        _aimTransform = transform.Find("Aim");
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        _bulletPrefab = _enemyStats.bulletPrefab;
        _bulletForce.BaseValue = _enemyStats.projectileSpeed;
        _attackRange.BaseValue = _enemyStats.attackRange;
        _damage.BaseValue = _enemyStats.damage;
        _enemyType = _enemyStats.enemyAttackType;
        _spread.BaseValue = _enemyStats.spread;
    }

    void Update()
    {
        HandleAim();
    }

    public void HandleAim()
    {
        if (_player != null)
        {
            Vector3 _lookDirection = (_player.position - transform.position).normalized;
            // This gets the angle required to rotate to the player using Atan2 and converts it into degrees
            float _angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg - 0f;
            _aimTransform.eulerAngles = new Vector3(0, 0, _angle);
            Vector3 _scale = Vector3.one;

            if (_angle < -90 || _angle > 90)
            {
                _scale.y = -1.0f;
                _scale.x = +1.0f;
            }
            else
            {
                _scale.y = +1.0f;
                _scale.x = -1.0f;
            }
            _aimTransform.localScale = _scale;
        }
        
    }

    public void Shoot()
    {
        if (_enemyType.Equals("Shooter"))
        {
            int pickPoint = Random.Range(0, _firePoint.Length);
            GetComponent<AudioSource>().Play();
            GameObject _bullet = Instantiate(_bulletPrefab, _firePoint[pickPoint].position, _firePoint[pickPoint].rotation);
            Bullet bullet = _bullet.GetComponent<Bullet>();
            bullet._damage = _damage.Value;
            bullet._isPlayerBullet = false;
            float accuracy = Random.Range(-_spread.Value, _spread.Value);
            _bullet.transform.Rotate(0, 0, accuracy);
            _bullet.GetComponent<Rigidbody2D>().AddForce(_bullet.transform.right * _bulletForce.Value, ForceMode2D.Impulse);
        }
        else if (_enemyType.Equals("Melee"))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_firePoint[0].position, _attackRange.Value);

            foreach (Collider2D col in hitEnemies)
            {
                PlayerDamageHandler _player = col.GetComponent<PlayerDamageHandler>();
                if (_player != null)
                {
                    _player.TakeDamage(_damage.Value / 2f);
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
        Gizmos.DrawWireSphere(_firePoint[0].position, _attackRange.Value);
    }
}
