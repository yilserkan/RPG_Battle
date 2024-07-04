using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Hero;
using RPGGame.SaveSystem;
using RPGGame.Game;
using System.Linq;
using RPGGame.CloudServices;
using System.Threading.Tasks;

namespace RPGGame.Player
{
    public static class PlayerData 
    {
        private static GameData _gameData;
        private static HeroSettingsContainer _heroSettingsContainer;

        public static HeroSettingsContainer HeroSettingsContainer => _heroSettingsContainer;

        private static Dictionary<string,Hero.Hero> _playerHeroes = new Dictionary<string, Hero.Hero>();
        private static IHeroFactory _heroFactory = new HeroFactory();

        public static void Initialize(HeroSettingsContainer heroSettingsContainer)
        {
            _heroSettingsContainer = heroSettingsContainer;
        }

        public static async Task SetPlayerHeroes()
        {
            _playerHeroes.Clear();
            var heroResponse = await HeroCloudRequests.LoadHeroData();
            var heroDatas = heroResponse.HeroDatas;
            for (int i = 0; i < heroDatas.Length; i++)
            {
                var hero  = _heroFactory.CreateHero(heroDatas[i]);
                _playerHeroes.Add(hero.Settings.ID, hero);
            }
        }

        public static List<Hero.Hero> GetPlayerHeroes()
        {
            return _playerHeroes.Values.ToList();
        }

        public static void SetGameData(GameData gameData)
        {
            _gameData = gameData;
        }

        public static GameData GetGameData()
        {
            return _gameData;
        }

        public static bool HasActiveLevelData()
        {
            var levelData = _gameData.ActiveLevelData;
            return levelData.PlayerHeroes.Length > 0 && levelData.EnemyHeroes.Length > 0;
        }

        public static void LocallyIncreasePlayedMatchCounts()
        {
            _gameData.PlayerMatchCount++;
        }
    }
}
