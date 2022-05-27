using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CG.SFRL.Characters
{
    public class Norman : MonoBehaviour
    {
        [SerializeField] private Image _imageCoolDownGrenade;
        [SerializeField] private TMP_Text _textCoolDownGrenade;

        [SerializeField] private Image _imageCoolDownGunShield;
        [SerializeField] private TMP_Text _textCoolDownGunShield;

        public GameObject _grenade;
        public GameObject _gunShield;

        public Animator _animator;

        public float _gunShieldDuration;

        float _maxGunShieldCooldown;
        float _maxGrenadeCooldown;
        float _gunShieldCooldown;
        float _grenadeCooldown;

        public BasicCharacter _characterStats;

        // Start is called before the first frame update
        void Start()
        {           
            _maxGunShieldCooldown = _characterStats.qCooldown;
            _maxGrenadeCooldown = _characterStats.eCooldown;            

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