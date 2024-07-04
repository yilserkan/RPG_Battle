using RPGGame.Pool;
using RPGGame.Game;
using RPGGame.Hero;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Player;

namespace RPGGame.Level
{
    public class GameHeroSpawner : MonoBehaviour
    {
        [SerializeField] private ObjectPoolSettings _heroFlyweightSettings;
        [SerializeField] private Transform _heroParentTransform;
        [SerializeField] private SpawnPointManager _spawnPointManager;

        private IGameHeroFactory _heroFactory;

        private void Awake()
        {
            _heroFactory = new GameHeroFactory(_heroFlyweightSettings, _heroParentTransform);
        }

        public GameHero[] CreateHeroes(GameHeroData[] gameHeroDatas)
        {
            var heroTeam = (HeroTeam)gameHeroDatas[0].HeroTeam;
            var heroes = new GameHero[gameHeroDatas.Length];
            for (int i = 0; i < gameHeroDatas.Length; i++)
            {
                var gameHero = _heroFactory.Create(PlayerData.HeroSettingsContainer, gameHeroDatas[i]);
                heroes[i] = gameHero;
                var spawnPoint = _spawnPointManager.GetSpawnPoint(heroTeam);
                heroes[i].SetSpawnPoint(spawnPoint);
            }

            return heroes;
        }
    }
}
