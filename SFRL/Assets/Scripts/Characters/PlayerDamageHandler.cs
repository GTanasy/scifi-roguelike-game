using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Characters
{
    public class PlayerDamageHandler : MonoBehaviour
    {
        float _maxHealth;
        float _currentHealth;
        float _maxShield;
        float _currentShield;

        public Animator _animator;
        
        float _shieldRegenRate; 

        Coroutine _regen;

        public HealthBar _healthBar;
        public ShieldBar _shieldBar;

        public BasicCharacter _characterStats;

        // Start is called before the first frame update
        void Start()
        {
            _maxHealth = _characterStats.health;
            _maxShield = _characterStats.shield;
            _shieldRegenRate = 1 / _characterStats.shieldRegenRate;            

            _currentHealth = _maxHealth;
            _healthBar.SetMaxHealth(_maxHealth);

            _currentShield = _maxShield;
            _shieldBar.SetMaxShield(_maxShield);

        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("Current Shield: " + _currentShield);
            Debug.Log("Current Health: " + _currentHealth);
            if (_currentHealth <= 0)
            {
                Die();
            }
        }
        public void TakeDamage(float damage)
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

        void Die()
        {
            StopAllCoroutines();
            Destroy(gameObject);
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
    }

}

