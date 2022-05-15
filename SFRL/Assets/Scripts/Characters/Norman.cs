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
        [SerializeField] float _sheildRegen = 5.0f;

        WaitForSeconds _shieldRegenRate = new WaitForSeconds(1.0f);

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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeHealthDamage(20);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                if(_currentShield < 0)
                {
                    TakeHealthDamage(10);
                }
                else
                {
                    TakeShieldDamage(10);
                }
                if (_regen != null)
                {
                    StopCoroutine(_regen);
                }
                _regen = StartCoroutine(RegenShield());
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


