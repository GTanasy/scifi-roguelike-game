using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Characters
{
    public class Norman : MonoBehaviour
    {
        [SerializeField] int _maxHealth = 100;
        [SerializeField] int _currentHealth;
        [SerializeField] int _maxShield = 100;
        [SerializeField] int _currentShield;

        public GameObject _grenade;

        WaitForSeconds _shieldRegenRate = new WaitForSeconds(0.1f);

        Coroutine _regen;

        public HealthBar _healthBar;
        public ShieldBar _shieldBar;

        // Start is called before the first frame update
        void Start()
        {
            _currentHealth = _maxHealth;
            _healthBar.SetMaxHealth(_maxHealth);

            _currentShield = _maxShield;
            _shieldBar.SetMaxShield(_maxShield);
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Instantiate(_grenade, transform.position, Quaternion.identity);
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
    }
}