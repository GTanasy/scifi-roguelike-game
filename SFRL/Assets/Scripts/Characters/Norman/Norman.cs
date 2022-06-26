using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CG.SFRL.Characters
{
    public class Norman : MonoBehaviour
    {
        public BasicCharacter characterStats;

        Image _imageCoolDownGunShield;
        TMP_Text _textCoolDownGunShield;

        Image _imageCoolDownGrenade;
        TMP_Text _textCoolDownGrenade;

        Shooting _shooting;
        PlayerInput _playerInput;

        public GameObject grenade;
        public GameObject gunShield;
        public GameObject piercingShotPrefab;

        public float gunShieldDuration;

        public Animator normanAnimator;
        public Animator rifleAnimator;

        [HideInInspector]
        public CharacterStat maxGunShieldCooldown;
        [HideInInspector]
        public CharacterStat maxGrenadeCooldown;
        float _gunShieldCooldown;
        float _grenadeCooldown;

        float _rightClickStartHeld;
        float _pDamage;

        void Awake()
        {
            _imageCoolDownGunShield = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Ability1/Button/Image").GetComponent<Image>();
            _textCoolDownGunShield = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Ability1/Button/Text").GetComponent<TMP_Text>();

            _imageCoolDownGrenade = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Ability2/Button/Image").GetComponent<Image>();
            _textCoolDownGrenade = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Ability2/Button/Text").GetComponent<TMP_Text>();

            _shooting = GetComponent<Shooting>();
            _playerInput = GetComponent<PlayerInput>();
        }

        // Start is called before the first frame update
        void Start()
        {
            maxGunShieldCooldown.BaseValue = characterStats.qCooldown;
            maxGrenadeCooldown.BaseValue = characterStats.eCooldown;            

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
        }
        
        void GunShieldCooldown()
        {
            if (_gunShieldCooldown > 0)
            {
                _gunShieldCooldown -= Time.deltaTime;
                _textCoolDownGunShield.text = Mathf.RoundToInt(_gunShieldCooldown).ToString();
                _imageCoolDownGunShield.fillAmount = _gunShieldCooldown / maxGunShieldCooldown.Value;
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
                _imageCoolDownGrenade.fillAmount = _grenadeCooldown / maxGrenadeCooldown.Value;
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
            if (_playerInput.EKey && _grenadeCooldown == 0)
            {
                _grenadeCooldown = maxGrenadeCooldown.Value;
                Instantiate(grenade, transform.position, Quaternion.identity);
                _textCoolDownGrenade.gameObject.SetActive(true);
            }

            if (_playerInput.QKey && _gunShieldCooldown == 0)
            {
                _gunShieldCooldown = maxGunShieldCooldown.Value;
                StartCoroutine(GunShieldUp());
                _textCoolDownGunShield.gameObject.SetActive(true);
            }

            if (_playerInput.RightClickHeld && _shooting.magCapacity >= 10)
            {
                _rightClickStartHeld = Time.time;
                rifleAnimator.SetBool("isCharging", true);
            }
            if (_playerInput.RightClickRelease && _shooting.magCapacity >= 10)
            {
                float _rightClickHeld = Time.time - _rightClickStartHeld;
                PiercingDamage(_rightClickHeld);
                ShootPiercing();
                rifleAnimator.SetBool("isCharging", false);
            }
        }
                    
        IEnumerator GunShieldUp()
        {
            yield return new WaitForSeconds(0);
            gunShield.SetActive(true);
            normanAnimator.SetFloat("GunShieldUp", 1);

            yield return new WaitForSeconds(gunShieldDuration);
            normanAnimator.SetFloat("GunShieldUp", -1);
            yield return new WaitForSeconds(0.2f);
            gunShield.SetActive(false);
        }

        void ShootPiercing()
        {
            if (_shooting.magCapacity > 10)
            {
                _shooting.magCapacity = _shooting.magCapacity - 10;
            }
            else
            {
                _shooting.magCapacity = 0;
            }
            GameObject _piercingShot = Instantiate(piercingShotPrefab, _shooting.firePoint.position, _shooting.firePoint.rotation);

            Rigidbody2D _rb = _piercingShot.GetComponent<Rigidbody2D>();

            _piercingShot.GetComponent<NormanPiercingShot>().damage = (int)_pDamage;

            _rb.AddForce(_shooting.firePoint.right * _shooting.bulletForce.Value, ForceMode2D.Impulse);
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
    }
}