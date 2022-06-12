using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Enemy
{
    public class EnemyDamageHandler : MonoBehaviour
    {
        public BasicEnemy _enemyStats;

        public CharacterStat _maxHealth;
        public float _currentHealth;
        public CharacterStat _maxShield;
        float _currentShield;

        public float _shieldRegenRate;

        Coroutine _regen;

        RectTransform bossHealthBar;

        public HealthBar _healthBar;
        public ShieldBar _shieldBar;

        public bool _hasShield;
        public bool isDead;
        public bool isBoss;

        bool _explosion;

        void Start()
        {
            if (isBoss == true)
            {
            bossHealthBar = GameObject.Find("/UI/BossBar").GetComponent<RectTransform>();
            
            if (bossHealthBar != null)
            {
                ActivateBar(bossHealthBar);
                _healthBar = bossHealthBar.gameObject.GetComponentInChildren<HealthBar>();
                _shieldBar = bossHealthBar.gameObject.GetComponentInChildren<ShieldBar>();
            }
            }
            _maxHealth.BaseValue = _enemyStats.health;
            _maxShield.BaseValue = _enemyStats.shield;
            _shieldRegenRate = 1 / _enemyStats.shieldRegenRate;

            _currentHealth = _maxHealth.Value;
            _healthBar.SetMaxHealth(_maxHealth.Value);

            _currentShield = _maxShield.Value;
            _shieldBar.SetMaxShield(_maxShield.Value);
            isDead = false;
        }

        private void Update()
        {
            Debug.Log("Boss Shield: " + _maxShield.Value);
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

            while (_currentShield < _maxShield.Value)
            {
                _currentShield++;
                _shieldBar.SetShield(_currentShield);
                yield return new WaitForSeconds(_shieldRegenRate);
            }
        }

        void ActivateBar(RectTransform bossHealthBar)
        {
            Vector3 _scale = new Vector3();
            _scale.x = 0.01f;
            _scale.y = 0.01f;
            _scale.z = 0.01f;
            bossHealthBar.localScale = _scale;
        }

        void DeactivateBar(RectTransform bossHealthBar)
        {
            Vector3 _scale = new Vector3();
            _scale.x = 0f;
            _scale.y = 0f;
            _scale.z = 0f;
            bossHealthBar.localScale = _scale;
        }
        void Die()
        {
            if (bossHealthBar != null)
            {
                DeactivateBar(bossHealthBar);
            }
            StopAllCoroutines();
            MoneyController.moneyController.credits += 100;
            Destroy(gameObject);
        }
    }
}


