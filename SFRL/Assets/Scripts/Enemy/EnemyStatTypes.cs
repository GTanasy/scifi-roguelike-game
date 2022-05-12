using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CG.SFRL.Enemy
{
    public class EnemyStatTypes : MonoBehaviour
    {
        [System.Serializable]
        public class Base
        {
            public float _atkSpeed, _atkRange, _attack, _health, _armor, _aggroRange, _speed;
        }
    }
}


