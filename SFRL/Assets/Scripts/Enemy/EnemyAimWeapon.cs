using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CG.SFRL.Characters;
using CG.SFRL.Enemy;

public class EnemyAimWeapon : MonoBehaviour
{
    public BasicEnemy enemyStats;

    Transform _aimTransform;
    Transform _player;

    RoundSpawner roundManager;

    public string enemyType;

    public Transform[] firePoint;

    public GameObject bulletPrefab;

    public CharacterStat bulletForce;
    public CharacterStat attackRange;
    public CharacterStat damage;
    public CharacterStat spread;

    void Awake()
    {
        _aimTransform = transform.Find("Aim");
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        roundManager = GameObject.Find("RoundManager").GetComponent<RoundSpawner>();
    }

    void Start()
    {
        bulletPrefab = enemyStats.bulletPrefab;
        bulletForce.BaseValue = enemyStats.projectileSpeed;
        attackRange.BaseValue = enemyStats.attackRange;
        damage.BaseValue = enemyStats.damage;
        damage.AddModifier(new StatModifier(roundManager.enemyStatsBuff, StatModType.PercentMult));
        enemyType = enemyStats.enemyAttackType;
        spread.BaseValue = enemyStats.spread;
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
        if (enemyType.Equals("Shooter"))
        {
            int pickPoint = Random.Range(0, firePoint.Length);
            GetComponent<AudioSource>().Play();
            GameObject _bullet = Instantiate(bulletPrefab, firePoint[pickPoint].position, firePoint[pickPoint].rotation);
            Bullet bullet = _bullet.GetComponent<Bullet>();
            bullet.damage = damage.Value;
            bullet.isPlayerBullet = false;
            float accuracy = Random.Range(-spread.Value, spread.Value);
            _bullet.transform.Rotate(0, 0, accuracy);
            _bullet.GetComponent<Rigidbody2D>().AddForce(_bullet.transform.right * bulletForce.Value, ForceMode2D.Impulse);
        }
        else if (enemyType.Equals("Melee"))
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(firePoint[0].position, attackRange.Value);
            GetComponent<AudioSource>().Play();
            foreach (Collider2D col in hitEnemies)
            {
                PlayerDamageHandler _player = col.GetComponent<PlayerDamageHandler>();
                if (_player != null)
                {
                    _player.TakeDamage(damage.Value / 2f);
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
        Gizmos.DrawWireSphere(firePoint[0].position, attackRange.Value);
    }
}
