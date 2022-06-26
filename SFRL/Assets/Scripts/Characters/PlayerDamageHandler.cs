using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Characters
{
    public class PlayerDamageHandler : MonoBehaviour, IDamageable
    {
        [HideInInspector]
        public CharacterStat maxHealth;
        [HideInInspector]
        public float currentHealth;
        [HideInInspector]
        public CharacterStat maxShield;
        [HideInInspector]
        public float currentShield;
        [HideInInspector]
        public CharacterStat shieldRegenRate;
        float _shieldRegen;

        Coroutine _regen;

        HealthBar _healthBar;
        ShieldBar _shieldBar;

        public BasicCharacter characterStats;

        void Awake()
        {
            _healthBar = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Health Bar").GetComponent<HealthBar>();
            _shieldBar = GameObject.Find("GameHandler/UI/Canvas/PlayerHUD/Shield Bar").GetComponent<ShieldBar>();
        }

        // Start is called before the first frame update
        void Start()
        {
            maxHealth.BaseValue = characterStats.health;            
            maxShield.BaseValue = characterStats.shield;
            shieldRegenRate.BaseValue = characterStats.shieldRegenRate;
            _shieldRegen = shieldRegenRate.Value;

            currentHealth = maxHealth.Value;
            _healthBar.SetMaxHealth(maxHealth.Value);

            currentShield = maxShield.Value;
            _shieldBar.SetMaxShield(maxShield.Value);
            
        }

        // Update is called once per frame
        void Update()
        {           

        }
        public void TakeDamage(float damage)
        {
            if (currentShield <= 0)
            {
                currentShield = 0;
                TakeHealthDamage(damage);
            }
            else
            {
                TakeShieldDamage(damage);
            }
            if (_regen != null)
            {
                StopCoroutine(_regen);
            }
            _regen = StartCoroutine(RegenShield());
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        void TakeHealthDamage(float damage)
        {
            currentHealth -= damage;
            _healthBar.SetHealth(currentHealth);
        }

        void TakeShieldDamage(float damage)
        {
            currentShield -= damage;            
            _shieldBar.SetShield(currentShield);
        }

        public void RefreshHealthBars()
        {
            _healthBar.SetMaxHealth(maxHealth.Value);
            _healthBar.SetHealth(currentHealth);
            _shieldBar.SetMaxShield(maxShield.Value);
            _shieldBar.SetShield(currentShield);
        }

        public void Heal()
        {
            currentHealth = maxHealth.Value;
            _healthBar.SetMaxHealth(maxHealth.Value);
        }

        void Die()
        {
            StopAllCoroutines();
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.Invoke("GameOver", 1.5f);
            Destroy(gameObject);
        }

        public IEnumerator RegenShield()
        {
            yield return new WaitForSeconds(2);
            while (currentShield < maxShield.Value)
            {
                currentShield++;
                _shieldBar.SetShield(currentShield);
                yield return new WaitForSeconds(1 / _shieldRegen);
            }
        }
    }

}

