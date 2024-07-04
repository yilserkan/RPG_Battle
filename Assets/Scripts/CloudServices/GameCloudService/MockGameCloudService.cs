using RPGGame.Config;
using RPGGame.Game;
using RPGGame.Hero;
using RPGGame.Player;
using RPGGame.SaveSystem;
using RPGGame.StateMachine;
using RPGGame.Stats;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.CloudServices
{
    public class MockGameCloudService : IGameCloudService
    {
        private IHeroFactory _heroFactory = new HeroFactory();

        private Dictionary<int, GameHeroData[]> _mockHeroes = new Dictionary<int, GameHeroData[]>();
   
        private GameData _gameData;
        private HeroSaveSystem _heroSaveSystem = new HeroSaveSystem();
        private GameSaveSystem _gameSaveSystem = new GameSaveSystem();

        public Task<string> CreateLevelData(string[] selectedHeroIds)
        {
            _mockHeroes.Clear();
            var allPlayerHeroesDatas = _heroSaveSystem.Load();
            var settings = PlayerData.HeroSettingsContainer;

            var playerHeroes = new GameHeroData[selectedHeroIds.Length];
            float selectedPlayersAvgLevel = 0;
            for (int i = 0; i < selectedHeroIds.Length; i++)
            {
                var heroData = FindHeroDataByID(allPlayerHeroesDatas, selectedHeroIds[i]);
                playerHeroes[i] = new GameHeroData(heroData);
                selectedPlayersAvgLevel += playerHeroes[i].Level;
            }
            selectedPlayersAvgLevel /= selectedHeroIds.Length;

            var randomEnemyCount = Random.Range(1, 4);
            var enemyHeroes = new GameHeroData[3];
            for (int i = 0; i < enemyHeroes.Length; i++)
            {
                var randomHero = _heroFactory.CreateRandomHeroData(HeroTeam.Enemy, enemyHeroes, (int)selectedPlayersAvgLevel);
                enemyHeroes[i] = new GameHeroData(randomHero);
            }

            CreateGameHeroDatas(playerHeroes);
            CreateGameHeroDatas(enemyHeroes);

            var levelData = new LevelData(playerHeroes, enemyHeroes);
            UpdateGameData(GameStates.PlayerTurn);

            var response = new CreateLevelDataResponse();
            response.IsSuccessfull = true;
            response.LevelData = levelData;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task<string> AttackEnemyPlayer(string selectedPlayerHero)
        {
            GameHeroData attackerHero = FindGameHeroDataByID(selectedPlayerHero, HeroTeam.Player);
            var receiverHero = FindRandomAliveHero(HeroTeam.Enemy);

            var damage = CalculateDamageForHero(attackerHero);
            DamageHero(receiverHero, damage);

            var gameCompleted = CheckIfGameCompleted(receiverHero.HeroTeam);
            if (gameCompleted)
            {
                OnGameCompleted(true);
            }
            else
            {
                UpdateGameData(GameStates.EnemyTurn);
            }

            var response = new AttackEnemyHeroResponse();
            response.IsSuccessfull = true;
            response.EnemyId = receiverHero.ID;
            response.Damage = damage;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task<string> SimulateEnemyAttack()
        {
            var attackerHero = FindRandomAliveHero(HeroTeam.Enemy);
            var receiverHero = FindRandomAliveHero(HeroTeam.Player);

            var damage = CalculateDamageForHero(attackerHero);
            DamageHero(receiverHero, damage);

            var gameCompleted = CheckIfGameCompleted(receiverHero.HeroTeam);
            if (gameCompleted)
            {
                OnGameCompleted(false);
            }
            else
            {
                UpdateGameData(GameStates.PlayerTurn);
            }

            var response = new SimulateEnemyAttackResponse();
            response.IsSuccessfull = true;
            response.ReceiverHeroID = receiverHero.ID;
            response.AttackerHeroID = attackerHero.ID;
            response.Damage = damage;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task<string> LoadGameData()
        {
            if (_gameSaveSystem.HasSaveFile())
            {
                _gameData = _gameSaveSystem.Load();

                bool isGameInProgress = _gameData.ActiveLevelData.CurrentState != (int)GameStates.None;
                if (isGameInProgress)
                {
                    var playerHeroDatas = _gameData.ActiveLevelData.PlayerHeroes;
                    var enemyHeroDatas = _gameData.ActiveLevelData.EnemyHeroes;
                    CreateGameHeroDatas(playerHeroDatas);
                    CreateGameHeroDatas(enemyHeroDatas);
                }
            }
            else
            {
                _gameData = new GameData();
                _gameData.PlayerMatchCount = 0;
                _gameData.ActiveLevelData = new LevelData();

                _gameSaveSystem.Save(_gameData);
            }

            var response = new LoadGameDataResponse();
            response.IsSuccessfull = true;
            response.GameData = _gameData;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        private void CreateGameHeroDatas(GameHeroData[] gameHeroesDatas)
        {
            _mockHeroes.Add(gameHeroesDatas[0].HeroTeam, gameHeroesDatas);
        }

        private HeroData FindHeroDataByID(HeroData[] heroDatas, string targetID)
        {
            for (int i = 0; i < heroDatas.Length; i++)
            {
                if (heroDatas[i].ID == targetID)
                    return heroDatas[i];
            }

            return null;
        }

        private GameHeroData FindGameHeroDataByID(string heroId, HeroTeam heroTeam)
        {
            var playerHeroes = _mockHeroes[(int)heroTeam];

            for (int i = 0; i < playerHeroes.Length; i++)
            {
                if (playerHeroes[i].ID == heroId)
                {
                    return playerHeroes[i];
                }
            }

            return null;
        }

        private GameHeroData FindRandomAliveHero(HeroTeam heroTeam)
        {
            var heroesGameDatas = _mockHeroes[(int)heroTeam];
            var aliveHeroeGameDatas = heroesGameDatas.Where(hero => hero.RemainingVitality > 0).ToArray();
            int randomHeroDataIndex = Random.Range(0, aliveHeroeGameDatas.Length);
            return aliveHeroeGameDatas[randomHeroDataIndex];
        }

        private void DamageHero(GameHeroData gameHeroData, float damage)
        {
            gameHeroData.RemainingVitality -= damage;
        }

        private float CalculateDamageForHero(GameHeroData heroData)
        {
            var heroDamage = GetHeroStatValue(heroData, StatTypes.Attack);
            return heroDamage;
        }
 
        private float GetHeroStatValue(GameHeroData gameHeroData, StatTypes statType)
        {
            var heroSettingsContainer = PlayerData.HeroSettingsContainer;
            if (!heroSettingsContainer.TryGetHeroSettings(gameHeroData.ID, out var settings)) return 0;

            var stats = new CharacterStat(settings.BaseStats, gameHeroData.Level);
            return stats.GetAttributeValue(statType);
        }

        private bool CheckIfGameCompleted(int receiverTeam)
        {
            var heroes = _mockHeroes[receiverTeam];
            var aliveHeroes = heroes.Where(hero => hero.RemainingVitality > 0).ToArray();
            return aliveHeroes.Length == 0;
        }

        private async void OnGameCompleted(bool playerWon)
        {
            if (playerWon)
            {
                IncreaseHeroExperiences();
            }

            _gameData.PlayerMatchCount++;
            if(_gameData.PlayerMatchCount % GameConfig.Data.ReceiveNewHeroInterval ==0)
            {
                await HeroCloudRequests.AddRandomHeroToPlayer();
            }

            ResetActiveGameData();
      
        }

        private async void IncreaseHeroExperiences()
        {
            var playerGameHeroes = _mockHeroes[(int)HeroTeam.Player];
            var heroesToIncreaseXp = playerGameHeroes.Where(hero => hero.RemainingVitality > 0).ToArray();
            var heroIds = heroesToIncreaseXp.Select(hero => hero.ID).ToArray();
            var request = new IncreaseHeroExperienceRequest() { HeroIds = heroIds };
            var increaseXpRequestJson = JsonUtility.ToJson(request);
            await HeroCloudRequests.IncreaseHeroExperiences(increaseXpRequestJson);
        }

        public void UpdateGameData(GameStates nextGameState)
        {
            var playerHeroeDatas = _mockHeroes[(int)HeroTeam.Player];
            var enemyHeroeDatas = _mockHeroes[(int)HeroTeam.Enemy];
            var levelData = new LevelData(playerHeroeDatas, enemyHeroeDatas, nextGameState);

            _gameData.ActiveLevelData = levelData;
            _gameSaveSystem.Save(_gameData);
        }

        public void ResetActiveGameData()
        {
            _gameData.ActiveLevelData = new LevelData();
            _gameSaveSystem.Save(_gameData);
        }
    }
}
