using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Enemy
{
    public class EnemyDamageHandler : MonoBehaviour
    {
        public BasicEnemy enemyStats;

        public CharacterStat maxHealth;
        public float currentHealth;
        public CharacterStat maxShield;
        float _currentShield;
        int _killCredits;

        public float shieldRegenRate;

        Coroutine _regen;

        RectTransform bossHealthBar;

        public HealthBar healthBar;
        public ShieldBar shieldBar;

        public bool hasShield;
        public bool isDead;
        public bool isBoss;

        GameManager _killTracker;
        RoundSpawner _roundManager;
        SoundManager _sounds;

        bool _explosion;

        void Awake()
        {
            _killTracker = GameObject.Find("GameManager").GetComponent<GameManager>();
            _roundManager = GameObject.Find("RoundManager").GetComponent<RoundSpawner>();
            _sounds = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        }

        void Start()
        {
            if (isBoss == true)
            {
            bossHealthBar = GameObject.Find("/UI/BossBar").GetComponent<RectTransform>();
            
            if (bossHealthBar != null)
            {
                ActivateBar(bossHealthBar);
                healthBar = bossHealthBar.gameObject.GetComponentInChildren<HealthBar>();
                shieldBar = bossHealthBar.gameObject.GetComponentInChildren<ShieldBar>();
            }
            }
            maxHealth.BaseValue = enemyStats.health;
            maxHealth.AddModifier(new StatModifier(_roundManager.enemyStatsBuff, StatModType.PercentMult));
            
            maxShield.BaseValue = enemyStats.shield;
            maxShield.AddModifier(new StatModifier(_roundManager.enemyStatsBuff, StatModType.PercentMult));
            shieldRegenRate = 1 / enemyStats.shieldRegenRate;

            currentHealth = maxHealth.Value;
            healthBar.SetMaxHealth(maxHealth.Value);

            _currentShield = maxShield.Value;
            shieldBar.SetMaxShield(maxShield.Value);
            _killCredits = enemyStats.killCredits;
            
            isDead = false;
        }

        private void Update()
        {
            
        }
        public void TakeDamage(float damage)
        {
            if (_currentShield <= 0)
            {
                hasShield = false;
                _currentShield = 0;
                TakeHealthDamage(damage);
            }
            else
            {
                hasShield = true;
                TakeShieldDamage(damage);
            }

            if (_regen != null)
            {
                StopCoroutine(_regen);
            }
            _regen = StartCoroutine(RegenShield());

            if(currentHealth <= 0)
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
            _currentShield -= damage;
            shieldBar.SetShield(_currentShield);
        }

        public void ExplosionDamage(int damage)
        {
            if (!_explosion)
            {
                _explosion = true;
                TakeDamage(damage);
                StartCoroutine(CoolDown(damage));
            }
        }
        IEnumerator CoolDown(int damage)
        {
            DamagePopup.Create(transform.position, damage, false, hasShield);
            yield return new WaitForSeconds(1.0f);
            _explosion = false;
        }

        IEnumerator RegenShield()
        {
            yield return new WaitForSeconds(5);

            while (_currentShield < maxShield.Value)
            {
                _currentShield++;
                shieldBar.SetShield(_currentShield);
                yield return new WaitForSeconds(shieldRegenRate);
            }
        }

        void ActivateBar(RectTransform bossHealthBar)
        {
            Vector3 _scale = new Vector3();
            _scale.x = 0.01f;
            _scale.y = 0.01f;
            _scale.z = 0.01f;
            bossHealthBar.localScale = _scale;
        }

        void DeactivateBar(RectTransform bossHealthBar)
        {
            Vector3 _scale = new Vector3();
            _scale.x = 0f;
            _scale.y = 0f;
            _scale.z = 0f;
            bossHealthBar.localScale = _scale;
        }
        void Die()
        {
            if (bossHealthBar != null)
            {
                DeactivateBar(bossHealthBar);
                _killTracker.bossKills++;
                _sounds.Play("BossExplosion");
            }
            else
            {
                _sounds.Play("EnemyDeath");
            }
            StopAllCoroutines();
            MoneyController.moneyController.credits += _killCredits;
            _killTracker.kills++;
            Destroy(gameObject);
        }
    }
}


