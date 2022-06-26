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

    TMP_Text _textAmmoCount;

    [HideInInspector]
    public Transform firePoint;
    Light2D _gunGlow;
    GameObject _bulletPrefab;

    public Animator animator;
    public AnimationClip reloadAnim;

    [HideInInspector]
    public CharacterStat reloadDuration;

    [HideInInspector]
    public CharacterStat bulletForce;
    [HideInInspector]
    public CharacterStat attackSpeed;
    float _timeBetweenShots;

    [HideInInspector]
    public CharacterStat bulletDamage;
    bool _isPlayerBullet;

    [HideInInspector]
    public CharacterStat maxMagCapacity;
    [HideInInspector]
    public int magCapacity;
    [HideInInspector]
    public CharacterStat criticalChance;

    PlayerInput _playerInput;

    bool _readyToShoot = true;
    bool _reloading = false;

    void Awake()
    {
        firePoint = transform.Find("Aim/Rifle/FirePoint").GetComponent<Transform>();
        _gunGlow = transform.Find("Aim/Rifle/GunGlow").GetComponent<Light2D>();
        _textAmmoCount = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/GunIcon/Text").GetComponent<TMP_Text>();
        _playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        maxMagCapacity.BaseValue = _weaponStats.magCapacity;
        bulletForce.BaseValue = _weaponStats.projectileSpeed;
        reloadDuration.BaseValue = _weaponStats.reloadTime;
        attackSpeed.BaseValue = _weaponStats.attackSpeed;
        _timeBetweenShots = 1 / attackSpeed.Value;
        maxMagCapacity.BaseValue = maxMagCapacity.Value;
        magCapacity = (int)maxMagCapacity.Value;
        criticalChance.BaseValue = _weaponStats.criticalChance;
        bulletDamage.BaseValue = _weaponStats.damage;
        _bulletPrefab = _weaponStats.playerBulletType;
        _isPlayerBullet = true;
    }

    // Update is called once per frame
    void Update()
    {
        ShootInput();
        _textAmmoCount.text = magCapacity + " / " + maxMagCapacity.Value;
    }

    void Shoot()
    {
        magCapacity--;
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

    void ShootInput()
    {
        if (_playerInput.LeftClick && _readyToShoot == true && _reloading == false)
        {
            if (magCapacity == 0)
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
        if (_playerInput.RKey && magCapacity != maxMagCapacity.Value)
        {
            animator.speed = reloadAnim.length / reloadDuration.Value;
            animator.SetBool("isReloading", true);
            StartCoroutine(Reload());
            _reloading = true;
        }
    }

    IEnumerator Reload()
    {
        _gunGlow.intensity = 0;
        yield return new WaitForSeconds(reloadDuration.Value);
        _gunGlow.intensity = 0.25f;
        magCapacity = (int)maxMagCapacity.Value;
        _reloading = false;
        animator.SetBool("isReloading", false);
    }
}