using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Enemy
{
    public class TestEnemyShooting : MonoBehaviour
    {
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
            StartCoroutine(ShootPlayer());         
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
    }
}


