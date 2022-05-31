using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Characters
{
    public class PlayerDamageHandler : MonoBehaviour
    {
        public CharacterStat _maxHealth;
        public float _currentHealth;
        public CharacterStat _maxShield;
        public float _currentShield;

        public Animator _animator;
        
        public CharacterStat _shieldRegenRate; 

        Coroutine _regen;

        public HealthBar _healthBar;
        public ShieldBar _shieldBar;

        public BasicCharacter _characterStats;

        // Start is called before the first frame update
        void Start()
        {
            _maxHealth.BaseValue = _characterStats.health;            
            _maxShield.BaseValue = _characterStats.shield;
            _shieldRegenRate.BaseValue = 1 / _characterStats.shieldRegenRate;            

            _currentHealth = _maxHealth.Value;
            _healthBar.SetMaxHealth(_maxHealth.Value);

            _currentShield = _maxShield.Value;
            _shieldBar.SetMaxShield(_maxShield.Value);
            
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("Current Health: " + _currentHealth);
            Debug.Log("Max Health: " + _maxHealth.Value);
            
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
            if (_currentHealth <= 0)
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

        void Die()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

        public IEnumerator RegenShield()
        {
            yield return new WaitForSeconds(2);
            while (_currentShield < _maxShield.Value)
            {
                _currentShield++;
                _shieldBar.SetShield(_currentShield);
                yield return new WaitForSeconds(_shieldRegenRate.Value);
            }
        }
    }

}

