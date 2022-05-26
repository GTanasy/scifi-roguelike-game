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
            Runt,
            Runner,
            Gunman
        };

        [Space(15)]
        [Header("Enemy Settings")]
        public enemyType _type;

        public GameObject _enemyPrefab;
        public GameObject _icon;

        [Space(15)]
        [Header("Enemy Base Stats")]
        [Space(15)]

        public float attackSpeed;
    }
}


