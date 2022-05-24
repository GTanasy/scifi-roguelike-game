using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CG.SFRL.Characters
{
    public class Norman : MonoBehaviour
    {
        float _maxHealth;
        float _currentHealth;
        float _maxShield;
        float _currentShield;

        [SerializeField] private Image _imageCoolDownGrenade;
        [SerializeField] private TMP_Text _textCoolDownGrenade;

        [SerializeField] private Image _imageCoolDownGunShield;
        [SerializeField] private TMP_Text _textCoolDownGunShield;

        public GameObject _grenade;
        public GameObject _gunShield;

        public Animator _animator;

        WaitForSeconds _shieldRegenRate = new WaitForSeconds(0.1f);
        public float _gunShieldDuration;

        float _maxGunShieldCooldown;
        float _maxGrenadeCooldown;
        float _gunShieldCooldown;
        float _grenadeCooldown;

        Coroutine _regen;

        public HealthBar _healthBar;
        public ShieldBar _shieldBar;

        public BasicCharacter _characterStats;

        // Start is called before the first frame update
        void Start()
        {
            _maxHealth = _characterStats.health;
            _maxShield = _characterStats.shield;

            _maxGunShieldCooldown = _characterStats.qCooldown;
            _maxGrenadeCooldown = _characterStats.eCooldown;

            _currentHealth = _maxHealth;
            _healthBar.SetMaxHealth(_maxHealth);

            _currentShield = _maxShield;
            _shieldBar.SetMaxShield(_maxShield);

            _textCoolDownGrenade.gameObject.SetActive(false);
            _imageCoolDownGrenade.fillAmount = 0.0f;
            _textCoolDownGunShield.gameObject.SetActive(false);
            _imageCoolDownGunShield.fillAmount = 0.0f;            
        }

        // Update is called once per frame
        void Update()
        {
            NormanInput();
            GunShieldCooldown();
            GrenadeCooldown();
            if(_currentHealth <= 0)
            {
                Die();
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

        void GunShieldCooldown()
        {
            if (_gunShieldCooldown > 0)
            {
                _gunShieldCooldown -= Time.deltaTime;
                _textCoolDownGunShield.text = Mathf.RoundToInt(_gunShieldCooldown).ToString();
                _imageCoolDownGunShield.fillAmount = _gunShieldCooldown / _maxGunShieldCooldown;
            }
            if (_gunShieldCooldown < 0)
            {
                _gunShieldCooldown = 0;
                _textCoolDownGunShield.gameObject.SetActive(false);
                _imageCoolDownGunShield.fillAmount = 0.0f;
            }
        }

        void GrenadeCooldown()
        {
            if (_grenadeCooldown > 0)
            {
                _grenadeCooldown -= Time.deltaTime;
                _textCoolDownGrenade.text = Mathf.RoundToInt(_grenadeCooldown).ToString();
                _imageCoolDownGrenade.fillAmount = _grenadeCooldown / _maxGrenadeCooldown;
            }
            if (_grenadeCooldown < 0)
            {
                _grenadeCooldown = 0;
                _textCoolDownGrenade.gameObject.SetActive(false);
                _imageCoolDownGrenade.fillAmount = 0.0f;
            }
        }

        void NormanInput()
        {
            if (Input.GetKeyDown(KeyCode.E) && _grenadeCooldown == 0)
            {
                _grenadeCooldown = _maxGrenadeCooldown;
                Instantiate(_grenade, transform.position, Quaternion.identity);
                _textCoolDownGrenade.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Q) && _gunShieldCooldown == 0)
            {
                _gunShieldCooldown = _maxGunShieldCooldown;
                StartCoroutine(GunShieldUp());
                _textCoolDownGunShield.gameObject.SetActive(true);
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