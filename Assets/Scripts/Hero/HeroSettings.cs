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
        [SerializeField] private Sprite _heroSprite;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;

        public string ID => _id;
        public string Name => _name;
        public CharacterBaseStats BaseStats => _baseStats;
        public Sprite HeroSprite => _heroSprite;
        public AnimatorOverrideController AnimatorOverrideController => _animatorOverrideController;
    }
}
