using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CG.SFRL.Characters;

    public class Shooting : MonoBehaviour
    {
        public BasicCharacter _weaponStats;

        [SerializeField] private NormanPiercingShot _pShot;

        [SerializeField] private TMP_Text _textAmmoCount;

        public Transform _firePoint;
        public GameObject _bulletPrefab;
        public GameObject _piercingShotPrefab;

        public Animator _animator;
        public AnimationClip _reloadAnim;

        float _reloadDuration;

        float _bulletForce;
        float _timeBetweenShots;

        float _rightClickStartHeld;

        float _pDamage;

        int _maxMagCapacity;
        int _magCapacity;

        bool _readyToShoot = true;
        bool _reloading = false;

        void Start()
        {
            _maxMagCapacity = _weaponStats.magCapacity;
            _bulletForce = _weaponStats.projectileSpeed;
            _reloadDuration = _weaponStats.reloadTime;
            _timeBetweenShots = 1 / _weaponStats.attackSpeed;
            _magCapacity = _maxMagCapacity;
        }

        // Update is called once per frame
        void Update()
        {
            ShootInput();
            _textAmmoCount.text = _magCapacity + " / " + _maxMagCapacity;
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