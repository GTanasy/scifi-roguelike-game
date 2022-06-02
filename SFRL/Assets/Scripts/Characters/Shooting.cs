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
        GameObject _bulletPrefab;
        public GameObject _piercingShotPrefab;

        public Animator _animator;
        public AnimationClip _reloadAnim;

        public CharacterStat _reloadDuration;

        public CharacterStat _bulletForce;
        public CharacterStat _attackSpeed;
        float _timeBetweenShots;

        float _rightClickStartHeld;

        public CharacterStat _bulletDamage;
        bool _isPlayerBullet;
        float _pDamage;

        public CharacterStat _maxMagCapacity;
        int _magCapacity;
        public CharacterStat _criticalChance;

        bool _readyToShoot = true;
        bool _reloading = false;

        void Start()
        {
            _maxMagCapacity.BaseValue = _weaponStats.magCapacity;
            _bulletForce.BaseValue = _weaponStats.projectileSpeed;
            _reloadDuration.BaseValue = _weaponStats.reloadTime;
            _attackSpeed.BaseValue = _weaponStats.attackSpeed;
            _timeBetweenShots = 1 / _attackSpeed.Value;
            _maxMagCapacity.BaseValue = _maxMagCapacity.Value;
            _magCapacity = (int)_maxMagCapacity.Value;
            _criticalChance.BaseValue = _weaponStats.criticalChance;
            _bulletDamage.BaseValue = _weaponStats.damage;
            _bulletPrefab = _weaponStats.playerBulletType;
            _isPlayerBullet = true;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log("Damage: " + _bulletDamage.Value);
            ShootInput();
            _textAmmoCount.text = _magCapacity + " / " + _maxMagCapacity.Value;            
        }

        void Shoot()
        {
            _magCapacity--;
            _readyToShoot = false;
            GameObject _bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

            Rigidbody2D _rb = _bullet.GetComponent<Rigidbody2D>();
            bool _isCriticalHit = Random.Range(0, 100) < (int)_criticalChance.Value;
            if (_isCriticalHit == true)
            {
                _bulletDamage.RemoveAllModifiersFromSource(this);
                _bullet.GetComponent<Bullet>().isCriticalHit = true;
                _bulletDamage.AddModifier(new StatModifier(1, StatModType.PercentAdd, this));
            }
            else
            {
                _bullet.GetComponent<Bullet>().isCriticalHit = false;
                _bulletDamage.RemoveAllModifiersFromSource(this);
            }
            _bullet.GetComponent<Bullet>()._damage = _bulletDamage.Value;
            _bullet.GetComponent<Bullet>()._isPlayerBullet = _isPlayerBullet;

            _rb.AddForce(_firePoint.right * _bulletForce.Value, ForceMode2D.Impulse);        
            
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

            _rb.AddForce(_firePoint.right * _bulletForce.Value, ForceMode2D.Impulse);
        }

        void ShootInput()
        {
            if (Input.GetButton("Fire1") && _readyToShoot == true && _reloading == false)
            {
                if (_magCapacity == 0)
                {
                    _animator.speed = _reloadAnim.length / _reloadDuration.Value;
                    _animator.SetBool("isReloading", true);
                    StartCoroutine(Reload());
                    _reloading = true;
                }
                else
                {
                    Shoot();
                }               
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
            if (Input.GetKeyDown(KeyCode.R) && _magCapacity != _maxMagCapacity.Value)
            {
                _animator.speed = _reloadAnim.length / _reloadDuration.Value;
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
            yield return new WaitForSeconds(_reloadDuration.Value);
            _magCapacity = (int)_maxMagCapacity.Value;
            _reloading = false;
            _animator.SetBool("isReloading", false);
        }
    }