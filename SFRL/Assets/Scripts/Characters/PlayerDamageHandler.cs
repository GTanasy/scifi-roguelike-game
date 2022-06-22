using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Characters
{
    public class PlayerDamageHandler : MonoBehaviour
    {
        public CharacterStat maxHealth;
        public float currentHealth;
        public CharacterStat maxShield;
        public float currentShield;
        
        public CharacterStat shieldRegenRate;
        float _shieldRegen;

        Coroutine _regen;

        public HealthBar healthBar;
        public ShieldBar shieldBar;

        public BasicCharacter characterStats;

        // Start is called before the first frame update
        void Start()
        {
            maxHealth.BaseValue = characterStats.health;            
            maxShield.BaseValue = characterStats.shield;
            shieldRegenRate.BaseValue = characterStats.shieldRegenRate;
            _shieldRegen = shieldRegenRate.Value;

            currentHealth = maxHealth.Value;
            healthBar.SetMaxHealth(maxHealth.Value);

            currentShield = maxShield.Value;
            shieldBar.SetMaxShield(maxShield.Value);
            
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
            healthBar.SetHealth(currentHealth);
        }

        void TakeShieldDamage(float damage)
        {
            currentShield -= damage;            
            shieldBar.SetShield(currentShield);
        }

        public void RefreshHealthBars()
        {
            healthBar.SetMaxHealth(maxHealth.Value);
            healthBar.SetHealth(currentHealth);
            shieldBar.SetMaxShield(maxShield.Value);
            shieldBar.SetShield(currentShield);
        }

        public void Heal()
        {
            currentHealth = maxHealth.Value;
            healthBar.SetMaxHealth(maxHealth.Value);
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
                shieldBar.SetShield(currentShield);
                yield return new WaitForSeconds(1 / _shieldRegen);
            }
        }
    }

}

