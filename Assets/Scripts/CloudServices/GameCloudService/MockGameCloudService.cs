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
        private IHeroFactory heroFactory = new HeroFactory();

        private Dictionary<int, GameHeroData[]> _mockHeroes = new Dictionary<int, GameHeroData[]>();
   
        private GameData gameData;

        public Task<string> CreateLevelData(string[] selectedHeroIds)
        {
            _mockHeroes.Clear();
            var allPlayerHeroesDatas = SaveSystemManager.Instance.HeroSaveSystem.Load();
            var settings = PlayerData.HeroSettingsContainer;

            var playerHeroes = new GameHeroData[selectedHeroIds.Length];
            for (int i = 0; i < selectedHeroIds.Length; i++)
            {
                var heroData = FindHeroDataByID(allPlayerHeroesDatas, selectedHeroIds[i]);
                settings.TryGetHeroSettings(heroData.ID, out var heroSettings);
                var hero = new Hero.Hero(heroSettings, heroData);
                playerHeroes[i] = new GameHeroData(hero);
            }

            var enemyHeroes = new GameHeroData[2];
            for (int i = 0; i < enemyHeroes.Length; i++)
            {
                var randomHero = heroFactory.CreateRandomHero(settings, HeroTeam.Enemy, false);
                enemyHeroes[i] = new GameHeroData(randomHero);
            }

            var levelData = new LevelData(
                CreateGameHeroDatas(playerHeroes),
                CreateGameHeroDatas(enemyHeroes));

            UpdateGameData(GameStates.PlayerTurn);

            var response = new CreateLevelDataResponse();
            response.IsSuccessfull = true;
            response.LevelData = levelData;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        private GameHeroData[] CreateGameHeroDatas(GameHeroData[] gameHeroesDatas)
        {
            _mockHeroes.Add(gameHeroesDatas[0].HeroTeam, gameHeroesDatas);

            return gameHeroesDatas;
        }

        private HeroData FindHeroDataByID(HeroData[] datas, string id)
        {
            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].ID == id)
                    return datas[i];
            }

            return null;
        }

        public Task<string> AttackEnemyPlayer(string selectedPlayerHero)
        {
            var playerHeroes = _mockHeroes[(int)HeroTeam.Player];
            GameHeroData attackerHero = null;
            for (int i = 0; i < playerHeroes.Length; i++)
            {
                if (playerHeroes[i].ID == selectedPlayerHero)
                {
                    attackerHero = playerHeroes[i];
                    break;
                }
            }

            var enemyHeroes = _mockHeroes[(int)HeroTeam.Enemy];
            var aliveEnemyHeroes = enemyHeroes.Where(hero => hero.RemainingVitality > 0).ToArray();
            int randomEnemyIndex = UnityEngine.Random.Range(0, aliveEnemyHeroes.Length);
            var receiverHero = aliveEnemyHeroes[randomEnemyIndex];

            var damage = CalculateDamageForHero(attackerHero);
            DamageHero(receiverHero, damage);

            var response = new AttackEnemyHeroResponse();
            response.IsSuccessfull = true;
            response.EnemyId = receiverHero.ID;
            response.Damage = damage;

            var gameCompleted = CheckIfGameCompleted(receiverHero.HeroTeam);
            if (gameCompleted)
            {
                OnGameCompleted(true);
            }
            else
            {
                UpdateGameData(GameStates.EnemyTurn);
            }

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        private void DamageHero(GameHeroData gameHeroData, float damage)
        {
            gameHeroData.RemainingVitality -= damage;
        }

        private float CalculateDamageForHero(GameHeroData heroData)
        {
            var heroSettingsContainer = PlayerData.HeroSettingsContainer;
            if (!heroSettingsContainer.TryGetHeroSettings(heroData.ID, out var settings)) return 0;

            var hero = new Hero.Hero(settings, heroData);
            var heroDamage = hero.Stats.CalculateAttributeValue(StatConstants.Attack);
            return heroDamage;
        }
 
        public Task<string> SimulateEnemyAttack()
        {
            var attackerHeroes = _mockHeroes[(int)HeroTeam.Enemy];
            var aliveAttackerHeroes = attackerHeroes.Where(hero => hero.RemainingVitality > 0).ToArray();
            int randomAttackerIndex = UnityEngine.Random.Range(0, aliveAttackerHeroes.Length);
            var attackerHero = aliveAttackerHeroes[randomAttackerIndex];

            var receiverHeroes = _mockHeroes[(int)HeroTeam.Player];
            var alivereceiverHeroes = receiverHeroes.Where(hero => hero.RemainingVitality > 0).ToArray();
            int randomreceiverIndex = UnityEngine.Random.Range(0, alivereceiverHeroes.Length);
            var receiverHero = alivereceiverHeroes[randomreceiverIndex];

            var damage = CalculateDamageForHero(attackerHero);
            DamageHero(receiverHero, damage);

            var response = new SimulateEnemyAttackResponse();
            response.IsSuccessfull = true;
            response.ReceiverHeroID = receiverHero.ID;
            response.AttackerHeroID = attackerHero.ID;
            response.Damage = damage;

            var gameCompleted = CheckIfGameCompleted(receiverHero.HeroTeam);

            if(gameCompleted)
            {
                OnGameCompleted(false);
            }
            else
            {
                UpdateGameData(GameStates.PlayerTurn);
            }

            return Task.FromResult(JsonUtility.ToJson(response));
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

            gameData.PlayerMatchCount++;
            if(gameData.PlayerMatchCount % 2 ==0)
            {
                await HeroCloudRequests.AddRandomHeroToPlayer();
            }

            ResetActiveGameData();
      
        }

        private void IncreaseHeroExperiences()
        {
            var heroSaveSystem = SaveSystemManager.Instance.HeroSaveSystem;

            var playerHeroDatas = heroSaveSystem.Load();

            var playerGameHeroes = _mockHeroes[(int)HeroTeam.Player];
            var heroesToIncreaseXp = playerGameHeroes.Where(hero => hero.RemainingVitality > 0).ToArray();

            for (int i = 0; i < playerHeroDatas.Length; i++)
            {
                for (int j = 0; j < heroesToIncreaseXp.Length; j++)
                {
                    if (playerHeroDatas[i].ID == heroesToIncreaseXp[j].ID)
                    {
                        playerHeroDatas[i].Experience++;

                        if(playerHeroDatas[i].Experience % 5 == 0)
                        {
                            playerHeroDatas[i].Level++;
                        }

                        break;
                    }
                }
            }

            heroSaveSystem.Save(playerHeroDatas);
        }

        public void UpdateGameData(GameStates nextGameState)
        {
            var playerHeroeDatas = _mockHeroes[(int)HeroTeam.Player];
            var enemyHeroeDatas = _mockHeroes[(int)HeroTeam.Enemy];
            var levelData = new LevelData(playerHeroeDatas, enemyHeroeDatas, nextGameState);

            var gameSaveSystem = SaveSystemManager.Instance.GameSaveSystem;
            gameData.ActiveLevelData = levelData;
            gameSaveSystem.Save(gameData);
        }

        public void ResetActiveGameData()
        {
            gameData.ActiveLevelData = new LevelData();

            var gameSaveSystem = SaveSystemManager.Instance.GameSaveSystem;
            gameSaveSystem.Save(gameData);
        }

        public Task<string> LoadGameData()
        {
            var gameSaveSystem = SaveSystemManager.Instance.GameSaveSystem;

            if(gameSaveSystem.HasSaveFile())
            {
                gameData = gameSaveSystem.Load();

                if(gameData.ActiveLevelData.CurrentState != (int)GameStates.None)
                {
                    var playerHeroDatas = gameData.ActiveLevelData.PlayerHeroes;
                    var enemyHeroDatas = gameData.ActiveLevelData.EnemyHeroes;
                    CreateGameHeroDatas(playerHeroDatas);
                    CreateGameHeroDatas(enemyHeroDatas);            
                }
            }
            else
            {
                gameData = new GameData();
                gameData.PlayerMatchCount = 0;
                gameData.ActiveLevelData = new LevelData();

                gameSaveSystem.Save(gameData);
            }

            var response = new LoadGameDataResponse();
            response.IsSuccessfull = true;
            response.GameData = gameData;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

    
    }
}
