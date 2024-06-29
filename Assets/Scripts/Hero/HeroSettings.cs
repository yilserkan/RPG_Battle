using RPGGame.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Hero
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Hero/Settings")]
    public class HeroSettings : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField] private CharacterBaseStats _baseStats;

        public string ID => _id;
        public string Name => _name;
        public CharacterBaseStats BaseStats => _baseStats;
    }
}
