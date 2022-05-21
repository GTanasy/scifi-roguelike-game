using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Shooting : MonoBehaviour
    {
        public Transform _firePoint;
        public GameObject _bulletPrefab;
        public GameObject _piercingShotPrefab;

        public Animator _animator;
        public AnimationClip _reloadAnim;

        public float _reloadDuration = 2.0f;

        public float _bulletForce = 20f;
        public float _timeBetweenShots = 0.5f;

        public int _maxMagCapacity;
        public int _magCapacity;

        bool _readyToShoot = true;
        bool _reloading = false;

        // Update is called once per frame
        void Update()
        {
            ShootInput();
        Debug.Log("Ammo: " + _magCapacity + "/" + _maxMagCapacity);
        }

        void Shoot()
        {
            _magCapacity--;
            _readyToShoot = false;
            GameObject _bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

            Rigidbody2D _rb = _bullet.GetComponent<Rigidbody2D>();

            _rb.AddForce(_firePoint.right * _bulletForce, ForceMode2D.Impulse);        
            
            _readyToShoot = false;
            Invoke("ResetShot", _timeBetweenShots);
        }
        
        void ResetShot()
        {
            _readyToShoot = true;
        }

        void ShootPiercing()
        {
            GameObject _piercingShot = Instantiate(_piercingShotPrefab, _firePoint.position, _firePoint.rotation);

            Rigidbody2D _rb = _piercingShot.GetComponent<Rigidbody2D>();

            _rb.AddForce(_firePoint.right * _bulletForce, ForceMode2D.Impulse);
        }

        void ShootInput()
        {
            if (Input.GetButton("Fire1") && _readyToShoot == true && _magCapacity > 0 && _reloading == false)
            {
                Shoot();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                ShootPiercing();
            }
            if (Input.GetKeyDown(KeyCode.R) && _magCapacity != _maxMagCapacity)
            {
                _animator.speed = _reloadAnim.length / _reloadDuration;
                _animator.SetBool("isReloading", true);                
                StartCoroutine(Reload());
                _reloading = true;
            }
        }

        IEnumerator Reload()
        {            
            yield return new WaitForSeconds(_reloadDuration);
            _magCapacity = _maxMagCapacity;
            _reloading = false;
            _animator.SetBool("isReloading", false);
        }
    }