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
        [SerializeField] float _sheildRegen = 5.0f;

        WaitForSeconds _shieldRegenRate = new WaitForSeconds(0.5f);

        Coroutine _regen;

        public HealthBar _healthBar;
        public ShieldBar _shieldBar;

        public Transform _firePoint;
        public Transform _player;

        public Camera _cam;

        Vector2 _playerPosition;

        public GameObject _bulletPrefab;
        public Rigidbody2D _rb;

        public float _bulletForce = 20f;

        public float _cooldown;

        void Start()
        {
            _currentHealth = _maxHealth;
            _healthBar.SetMaxHealth(_maxHealth);

            _currentShield = _maxShield;
            _shieldBar.SetMaxShield(_maxShield);

            StartCoroutine(ShootPlayer());         
        }

        private void Update()
        {
            Debug.Log("Health: " + _currentHealth);
            Debug.Log("Shield: " + _currentShield);
        }

        void FixedUpdate()
        {           
            Vector2 _targetPosition = _player.position - _rb.transform.position;           

            float _angle = Mathf.Atan2(_targetPosition.y, _targetPosition.x) * Mathf.Rad2Deg - 0f;
            _rb.rotation = _angle;          
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


