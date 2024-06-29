using RPGGame.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionManager : MonoBehaviour
    {
        [SerializeField] private HeroSelectionSlot[] _heroSlots;

        private void Start()
        {
            InitializeSlots();
        }

        private void InitializeSlots()
        {
            var heroes = PlayerData.GetPlayerHeroes();

            for (int i = 0; i < _heroSlots.Length; i++)
            {
                var hero = heroes.Length > i ? heroes[i] : null;
                _heroSlots[i].SetupSlot(hero);
            }
        }
    }
}

