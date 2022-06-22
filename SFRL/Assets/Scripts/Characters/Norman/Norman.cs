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

        public GameObject grenade;
        public GameObject gunShield;

        public Animator animator;

        public float gunShieldDuration;

        public CharacterStat maxGunShieldCooldown;
        public CharacterStat maxGrenadeCooldown;
        float _gunShieldCooldown;
        float _grenadeCooldown;

        public BasicCharacter characterStats;

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
            if (Input.GetKeyDown(KeyCode.E) && _grenadeCooldown == 0)
            {
                _grenadeCooldown = maxGrenadeCooldown.Value;
                Instantiate(grenade, transform.position, Quaternion.identity);
                _textCoolDownGrenade.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Q) && _gunShieldCooldown == 0)
            {
                _gunShieldCooldown = maxGunShieldCooldown.Value;
                StartCoroutine(GunShieldUp());
                _textCoolDownGunShield.gameObject.SetActive(true);
            }
        }
                    
        IEnumerator GunShieldUp()
        {
            yield return new WaitForSeconds(0);
            gunShield.SetActive(true);
            animator.SetFloat("GunShieldUp", 1);

            yield return new WaitForSeconds(gunShieldDuration);
            animator.SetFloat("GunShieldUp", -1);
            yield return new WaitForSeconds(0.2f);
            gunShield.SetActive(false);
        }
    }
}