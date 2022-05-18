using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Enemy
{
    public class TestEnemy : MonoBehaviour
    {

        [SerializeField] int _maxHealth = 100;
        [SerializeField] int _currentHealth;
        [SerializeField] int _maxShield = 100;
        [SerializeField] int _currentShield;

        WaitForSeconds _shieldRegenRate = new WaitForSeconds(0.5f);

        Coroutine _regen;

        public HealthBar _healthBar;
        public ShieldBar _shieldBar;

        public Transform _firePoint;
        public Transform _player;

        public GameObject _bulletPrefab;

        public float _bulletForce = 20f;

        public float _cooldown;

        private bool _explosion;

        void Start()
        {
            _currentHealth = _maxHealth;
            _healthBar.SetMaxHealth(_maxHealth);

            _currentShield = _maxShield;
            _shieldBar.SetMaxShield(_maxShield);

            StartCoroutine(ShootPlayer());              
        }

        IEnumerator ShootPlayer()
        {
                yield return new WaitForSeconds(_cooldown);
            if(_player != null)
            {                          
                GameObject _bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
                _bullet.GetComponent<Rigidbody2D>().AddForce(_firePoint.right * _bulletForce, ForceMode2D.Impulse);

                StartCoroutine(ShootPlayer());
            }

        }

        public void TakeDamage(int damage)
        {
            if (_currentShield <= 0)
            {
                _currentShield = 0;
                TakeHealthDamage(damage);
            }
            else
            {
                TakeShieldDamage(damage);
            }

            if (_regen != null)
            {
                StopCoroutine(_regen);
            }
            _regen = StartCoroutine(RegenShield());

            if(_currentHealth <= 0)
            {
                Die();
            }
        }

        void TakeHealthDamage(int damage)
        {
            _currentHealth -= damage;
            _healthBar.SetHealth(_currentHealth);
        }

        void TakeShieldDamage(int damage)
        {
            _currentShield -= damage;
            _shieldBar.SetShield(_currentShield);
        }

        public void ExplosionDamage(int damage)
        {
            if (!_explosion)
            {
                _explosion = true;
                TakeDamage(damage);
                StartCoroutine(CoolDown());
            }
        }

        IEnumerator CoolDown()
        {
            yield return new WaitForSeconds(1.0f);
            _explosion = false;
        }

        IEnumerator RegenShield()
        {
            yield return new WaitForSeconds(2);

            while (_currentShield < _maxShield)
            {
                _currentShield++;
                _shieldBar.SetShield(_currentShield);
                yield return _shieldRegenRate;
            }
        }

        void Die()
        {
            Destroy(gameObject);
            StopAllCoroutines();
        }
    }
}


