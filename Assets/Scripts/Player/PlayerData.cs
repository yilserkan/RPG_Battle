using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Hero;
using RPGGame.SaveSystem;

namespace RPGGame.Player
{
    public static class PlayerData 
    {
        private static Hero.Hero[] _playerHeroes;

        public static void SetPlayerHeroes(Hero.Hero[] heroes)
        {
            _playerHeroes = heroes;
            SaveSystemManager.Instance.HeroSaveSystem.Save();
        }

        public static Hero.Hero[] GetPlayerHeroes()
        {
            return _playerHeroes;
        }
    }
}
