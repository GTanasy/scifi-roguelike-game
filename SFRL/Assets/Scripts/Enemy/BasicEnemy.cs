using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Enemy
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Create New Basic Enemy")]
    public class BasicEnemy : ScriptableObject
    {
        public enum enemyType
        {
            Walker,
            Runner,
            PistolBot,
            RifleBot,
            SniperBot,
            DemoBot,
            MachineGunnerBot,
            ProtectorBot,
            DroneBot,
            TankBot
        };

        [Space(15)]
        [Header("Enemy Settings")]
        public enemyType _type;

        public GameObject _enemyPrefab;
        public GameObject _icon;

        [Space(15)]
        [Header("Enemy Base Stats")]
        [Space(15)]
       
        public bool canFly;       
        public float health;
        public float shield;
        public float shieldRegenRate;
        public float armor;
        public int killCredits;
        

        [Space(15)]
        [Header("Enemy AI Stats")]
        [Space(15)]

        public float speed;

        [Space(15)]
        [Header("Enemy Attack Stats")]
        [Space(15)]

        public GameObject bulletPrefab;
        public string enemyAttackType;
        public float damage;
        public float attackSpeed;
        public float attackRange;
        public float engagementRange;
        public float projectileSpeed;
        public int spread;
    }
}


