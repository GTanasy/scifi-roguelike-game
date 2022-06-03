using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Enemy
{
    public class EnemyDamageHandler : MonoBehaviour
    {
        public BasicEnemy _enemyStats;

        public float _maxHealth;
        public float _currentHealth;
        float _maxShield;
        float _currentShield;

        float _shieldRegenRate;

        Coroutine _regen;

        public HealthBar _healthBar;
        public ShieldBar _shieldBar;

        public bool _hasShield;

        bool _explosion;

        void Start()
        {
            _maxHealth = _enemyStats.health;
            _maxShield = _enemyStats.shield;
            _shieldRegenRate = _enemyStats.shieldRegenRate;

            _currentHealth = _maxHealth;
            _healthBar.SetMaxHealth(_maxHealth);

            _currentShield = _maxShield;
            _shieldBar.SetMaxShield(_maxShield);              
        }
        public void TakeDamage(float damage)
        {
            if (_currentShield <= 0)
            {
                _hasShield = false;
                _currentShield = 0;
                TakeHealthDamage(damage);
            }
            else
            {
                _hasShield = true;
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
        void TakeHealthDamage(float damage)
        {
            _currentHealth -= damage;
            _healthBar.SetHealth(_currentHealth);
        }

        void TakeShieldDamage(float damage)
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
            DamagePopup.Create(transform.position, 80, false, _hasShield);
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
                yield return new WaitForSeconds(_shieldRegenRate);
            }
        }
        void Die()
        {
            Destroy(gameObject);
            StopAllCoroutines();
        }
    }
}


