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
        
        public GameObject _playerPrefab;
        public GameObject _icon;        

        [Space(15)]
        [Header("Character Base Stats")]
        [Space(15)]

        public CharacterStatTypes.Base _baseStats;

    }
}


