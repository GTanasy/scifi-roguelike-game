using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Create New Basic Character")]
    public class BasicCharacter : ScriptableObject
    {
        public enum characterType
        {
            Norman,
            Hacker,
            Tank,
            Marksmen,
            Ninja,
            Vampire,
            Engineer
        };

        [Space(15)]
        [Header("Character Settings")]
        public characterType _type;

        public GameObject _characterPrefab;
        public GameObject _icon;

        [Space(15)]
        [Header("Character Base Stats")]
        [Space(15)]
        [Header("Character Main Stats")]
        [Space (15)]

        public float health;
        public float shield;
        public float shieldRegenRate;
        public float armor;
        public float qCooldown;
        public float eCooldown;

        [Space(15)]
        [Header("Weapon Stats")]
        [Space(15)]

        public GameObject playerBulletType;
        public int criticalChance;
        public float damage;
        public float attackSpeed;
        public float attackRange;
        public float projectileSpeed;
        public float reloadTime;
        public int magCapacity;

        [Space(15)]
        [Header("Move Stats")]
        [Space(15)]

        public float dashCooldown;
        public float dashLength;
        public float dashSpeed;
        public float normalSpeed;
        
        

        
    }
}


