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

        public GameObject _gunShield;

        WaitForSeconds _shieldRegenRate = new WaitForSeconds(0.1f);

        float _gunShieldCooldown = 10.0f;

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

            if (Input.GetKeyDown(KeyCode.Q) && _gunShieldCooldown == 0)
            {
                _gunShieldCooldown = 10;
                StartCoroutine(GunShieldUp());               
            }
            if (_gunShieldCooldown > 0)
            {
                _gunShieldCooldown -= Time.deltaTime;
            }
            if (_gunShieldCooldown < 0)
            {
                _gunShieldCooldown = 0;
            }
            
            Debug.Log("GunShield Cooldown: " + _gunShieldCooldown);
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
        void GunShieldDown()
        {
            _gunShield.SetActive(false);
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

        IEnumerator GunShieldUp()
        {
            yield return new WaitForSeconds(0);
            _gunShield.SetActive(true);

            yield return new WaitForSeconds(5);
            _gunShield.SetActive(false);
        }
    }
}