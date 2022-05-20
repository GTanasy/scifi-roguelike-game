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

        public Animator _animator;

        WaitForSeconds _shieldRegenRate = new WaitForSeconds(0.1f);
        public float _gunShieldDuration;

        float _gunShieldCooldown = 0;

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
            NormanInput();
            GunShieldCooldown();
            if(_currentHealth <= 0)
            {
                Die();
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

        void GunShieldCooldown()
        {
            if (_gunShieldCooldown > 0)
            {
                _gunShieldCooldown -= Time.deltaTime;
            }
            if (_gunShieldCooldown < 0)
            {
                _gunShieldCooldown = 0;
            }
        }

        void NormanInput()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Instantiate(_grenade, transform.position, Quaternion.identity);
            }

            if (Input.GetKeyDown(KeyCode.Q) && _gunShieldCooldown == 0)
            {
                _gunShieldCooldown = 10.0f;
                StartCoroutine(GunShieldUp());
            }
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
                yield return _shieldRegenRate;
            }
        }

        IEnumerator GunShieldUp()
        {
            yield return new WaitForSeconds(0);
            _gunShield.SetActive(true);
            _animator.SetFloat("GunShieldUp", 1);

            yield return new WaitForSeconds(_gunShieldDuration);
            _animator.SetFloat("GunShieldUp", -1);
            yield return new WaitForSeconds(0.2f);
            _gunShield.SetActive(false);
        }
    }
}