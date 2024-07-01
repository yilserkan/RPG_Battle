using RPGGame.Pool;
using RPGGame.Hero;
using RPGGame.HeroSelection;
using RPGGame.Level;
using RPGGame.Player;
using RPGGame.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.StateMachine;

namespace RPGGame.Game
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private HeroSettingsContainer _enemySettings;
        [SerializeField] private GameHeroSpawner _levelManager;
        [SerializeField] private GameStateMachine _stateManager;

        private IHeroFactory _heroFactory = new HeroFactory();

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void HandleOnStartGame(List<Hero.Hero> selectedHeroes)
        {
            var enemy = CreateEnemies(2);
            var levelData = CreateLevelData(selectedHeroes.ToArray(), enemy);
            _stateManager.StartLevel(levelData);
        }

        private Hero.Hero[] CreateEnemies(int enemyCount)
        {
            var enemies = new Hero.Hero[enemyCount];
            for (int i = 0; i < enemyCount; i++)
            {
                enemies[i] = _heroFactory.CreateRandomHero(_enemySettings, HeroTeam.Enemy);
            }
            return enemies;
        }

        private LevelData CreateLevelData(Hero.Hero[] playerHeroes, Hero.Hero[] enemyHeroes)
        {
            var levelData = new LevelData(
                CreateGameHeroDatas(playerHeroes), 
                CreateGameHeroDatas(enemyHeroes), 
                1);

            return levelData;
        }

        private GameHeroData[] CreateGameHeroDatas(Hero.Hero[] heroes)
        {
            var gameHeroesDatas = new GameHeroData[heroes.Length];
            for (int i = 0; i < heroes.Length; i++)
            {
                gameHeroesDatas[i] = new GameHeroData(heroes[i]);
            }
            return gameHeroesDatas;
        }

        private void AddListeners() 
        {
            HeroSelectionManager.OnStartGameEvent += HandleOnStartGame;
        }

        private void RemoveListeners()
        {
            HeroSelectionManager.OnStartGameEvent -= HandleOnStartGame;
        }
    }
}
