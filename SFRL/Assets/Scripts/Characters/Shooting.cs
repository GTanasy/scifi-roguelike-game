using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using TMPro;
using CG.SFRL.Characters;

public class Shooting : MonoBehaviour
{
    public BasicCharacter _weaponStats;

    [SerializeField] private NormanPiercingShot _pShot;

    [SerializeField] private TMP_Text _textAmmoCount;

    public Transform firePoint;
    public Light2D gunGlow;
    GameObject _bulletPrefab;
    public GameObject piercingShotPrefab;

    public Animator animator;
    public AnimationClip reloadAnim;

    public CharacterStat reloadDuration;

    public CharacterStat bulletForce;
    public CharacterStat attackSpeed;
    float _timeBetweenShots;

    float _rightClickStartHeld;

    public CharacterStat bulletDamage;
    bool _isPlayerBullet;
    float _pDamage;

    public CharacterStat maxMagCapacity;
    int _magCapacity;
    public CharacterStat criticalChance;

    bool _readyToShoot = true;
    bool _reloading = false;

    void Start()
    {
        maxMagCapacity.BaseValue = _weaponStats.magCapacity;
        bulletForce.BaseValue = _weaponStats.projectileSpeed;
        reloadDuration.BaseValue = _weaponStats.reloadTime;
        attackSpeed.BaseValue = _weaponStats.attackSpeed;
        _timeBetweenShots = 1 / attackSpeed.Value;
        maxMagCapacity.BaseValue = maxMagCapacity.Value;
        _magCapacity = (int)maxMagCapacity.Value;
        criticalChance.BaseValue = _weaponStats.criticalChance;
        bulletDamage.BaseValue = _weaponStats.damage;
        _bulletPrefab = _weaponStats.playerBulletType;
        _isPlayerBullet = true;
    }

    // Update is called once per frame
    void Update()
    {
        ShootInput();
        _textAmmoCount.text = _magCapacity + " / " + maxMagCapacity.Value;
    }

    void Shoot()
    {
        _magCapacity--;
        _readyToShoot = false;
        GetComponent<AudioSource>().Play();
        GameObject _bullet = Instantiate(_bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D _rb = _bullet.GetComponent<Rigidbody2D>();
        bool _isCriticalHit = Random.Range(0, 100) < (int)criticalChance.Value;
        if (_isCriticalHit == true)
        {
            bulletDamage.RemoveAllModifiersFromSource(this);
            _bullet.GetComponent<Bullet>().isCriticalHit = true;
            bulletDamage.AddModifier(new StatModifier(1, StatModType.PercentAdd, this));
        }
        else
        {
            _bullet.GetComponent<Bullet>().isCriticalHit = false;
            bulletDamage.RemoveAllModifiersFromSource(this);
        }
        _bullet.GetComponent<Bullet>().damage = bulletDamage.Value;
        _bullet.GetComponent<Bullet>().isPlayerBullet = _isPlayerBullet;

        _rb.AddForce(firePoint.right * bulletForce.Value, ForceMode2D.Impulse);

        _readyToShoot = false;
        _timeBetweenShots = 1 / attackSpeed.Value;
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
        GameObject _piercingShot = Instantiate(piercingShotPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D _rb = _piercingShot.GetComponent<Rigidbody2D>();

        _piercingShot.GetComponent<NormanPiercingShot>().damage = (int)_pDamage;

        _rb.AddForce(firePoint.right * bulletForce.Value, ForceMode2D.Impulse);
    }

    void ShootInput()
    {
        if (Input.GetButton("Fire1") && _readyToShoot == true && _reloading == false)
        {
            if (_magCapacity == 0)
            {
                animator.speed = reloadAnim.length / reloadDuration.Value;
                animator.SetBool("isReloading", true);
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
            animator.SetBool("isCharging", true);
        }
        if (Input.GetButtonUp("Fire2") && _magCapacity >= 10)
        {
            float _rightClickHeld = Time.time - _rightClickStartHeld;
            PiercingDamage(_rightClickHeld);
            ShootPiercing();
            animator.SetBool("isCharging", false);
        }
        if (Input.GetKeyDown(KeyCode.R) && _magCapacity != maxMagCapacity.Value)
        {
            animator.speed = reloadAnim.length / reloadDuration.Value;
            animator.SetBool("isReloading", true);
            StartCoroutine(Reload());
            _reloading = true;
        }
    }

    void PiercingDamage(float time)
    {
        if (time >= 4.0f)
        {
            _pDamage = 300.0f;
        }
        else
        {
            _pDamage = time * 75.0f;
        }
    }

    IEnumerator Reload()
    {
        gunGlow.intensity = 0;
        yield return new WaitForSeconds(reloadDuration.Value);
        gunGlow.intensity = 0.25f;
        _magCapacity = (int)maxMagCapacity.Value;
        _reloading = false;
        animator.SetBool("isReloading", false);
    }
}