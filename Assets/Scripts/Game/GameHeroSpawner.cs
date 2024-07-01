using RPGGame.Pool;
using RPGGame.Game;
using RPGGame.Hero;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Level
{
    public class GameHeroSpawner : MonoBehaviour
    {
        [SerializeField] private HeroSettingsContainer _enemySettings;
        [SerializeField] private HeroSettingsContainer _heroSettings;
        [SerializeField] private ObjectPoolSettings _heroFlyweightSettings;

        [SerializeField] private Transform _heroParentTransform;
        [SerializeField] private SpawnPointManager _spawnPointManager;

        private IGameHeroFactory _heroFactory;

        private void Start()
        {
            _heroFactory = new GameHeroFactory(_heroFlyweightSettings, _heroParentTransform);
        }

        public GameHero[] CreateHeroes(GameHeroData[] gameHeroDatas)
        {
            var heroTeam = (HeroTeam)gameHeroDatas[0].HeroTeam;
            var settings = heroTeam == (int)HeroTeam.Player ? _heroSettings : _enemySettings;
            var heroes = new GameHero[gameHeroDatas.Length];
            for (int i = 0; i < gameHeroDatas.Length; i++)
            {
                var gameHero = _heroFactory.Create(settings, gameHeroDatas[i]);
                heroes[i] = gameHero;
                heroes[i].transform.position = _spawnPointManager.GetSpawnPoint(heroTeam);
            }

            return heroes;
        }
    }
}
