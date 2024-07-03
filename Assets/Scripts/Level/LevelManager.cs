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
using RPGGame.CloudServices;

namespace RPGGame.Game
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private HeroSettingsContainer _heroSettings;
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

        private void Start()
        {
            if (PlayerData.HasActiveLevelData())
                _stateManager.StartLevel(PlayerData.GetGameData().ActiveLevelData);
        }

        private async void HandleOnStartGame(List<Hero.Hero> selectedHeroes)
        {
            var selectedHeroIds = new string[selectedHeroes.Count];
            for (int i = 0; i < selectedHeroIds.Length; i++)
            {
                selectedHeroIds[i] = selectedHeroes[i].Settings.ID;
            }

            var data = await GameCloudRequests.CreateLevelData(selectedHeroIds);

            //var enemy = CreateEnemies(2);
            //var levelData = CreateLevelData(selectedHeroes.ToArray(), enemy);
            _stateManager.StartLevel(data.LevelData);
        }

        private Hero.Hero[] CreateEnemies(int enemyCount)
        {
            var enemies = new Hero.Hero[enemyCount];
            for (int i = 0; i < enemyCount; i++)
            {
                enemies[i] = _heroFactory.CreateRandomHero(_heroSettings, HeroTeam.Enemy, false);
            }
            return enemies;
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
