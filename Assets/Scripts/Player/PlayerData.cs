using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Hero;
using RPGGame.SaveSystem;

namespace RPGGame.Player
{
    public static class PlayerData 
    {
        private static List<Hero.Hero> _playerHeroes = new List<Hero.Hero>();

        public static void SetPlayerHeroes(List<Hero.Hero> heroes)
        {
            _playerHeroes = heroes;
            SaveSystemManager.Instance.HeroSaveSystem.Save();
        }

        public static void AddHero(Hero.Hero hero)
        {
            _playerHeroes.Add(hero);
            SaveSystemManager.Instance.HeroSaveSystem.Save();
        }

        public static List<Hero.Hero> GetPlayerHeroes()
        {
            return _playerHeroes;
        }
    }
}
