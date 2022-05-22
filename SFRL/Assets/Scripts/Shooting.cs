using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class Shooting : MonoBehaviour
    {
        [SerializeField] private NormanPiercingShot _pShot;

        public Transform _firePoint;
        public GameObject _bulletPrefab;
        public GameObject _piercingShotPrefab;

        public Animator _animator;
        public AnimationClip _reloadAnim;

        public float _reloadDuration = 2.0f;

        public float _bulletForce = 20f;
        public float _timeBetweenShots = 0.5f;

        private float _rightClickStartHeld;

        private float _pDamage;

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
            if (_magCapacity > 10)
            {
                _magCapacity = _magCapacity - 10;
            }
            else
            {
                _magCapacity = 0;
            }
            GameObject _piercingShot = Instantiate(_piercingShotPrefab, _firePoint.position, _firePoint.rotation);

            Rigidbody2D _rb = _piercingShot.GetComponent<Rigidbody2D>();

            _piercingShot.GetComponent<NormanPiercingShot>()._damage = (int)_pDamage;

            _rb.AddForce(_firePoint.right * _bulletForce, ForceMode2D.Impulse);
        }

        void ShootInput()
        {
            if (Input.GetButton("Fire1") && _readyToShoot == true && _magCapacity > 0 && _reloading == false)
            {
                Shoot();
            }
            if (Input.GetButtonDown("Fire2") && _magCapacity >= 10)
            {
                _rightClickStartHeld = Time.time;
                _animator.SetBool("isCharging", true);
            }
            if (Input.GetButtonUp("Fire2") && _magCapacity >= 10)
            {
                float _rightClickHeld = Time.time - _rightClickStartHeld;
                PiercingDamage(_rightClickHeld);
                ShootPiercing();
                _animator.SetBool("isCharging", false);
            }
            if (Input.GetKeyDown(KeyCode.R) && _magCapacity != _maxMagCapacity)
            {
                _animator.speed = _reloadAnim.length / _reloadDuration;
                _animator.SetBool("isReloading", true);                
                StartCoroutine(Reload());
                _reloading = true;
            }
        }

        void PiercingDamage(float time)
        {
            if (time >= 4.0f)
            {
                _pDamage = 100.0f;
            }
            else
            {
                _pDamage = time * 25.0f;
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