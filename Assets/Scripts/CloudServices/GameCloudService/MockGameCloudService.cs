using RPGGame.Game;
using RPGGame.Hero;
using RPGGame.Player;
using RPGGame.SaveSystem;
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

        public Task<string> CreateLevelData(string[] selectedHeroIds)
        {
            var allPlayerHeroesDatas = SaveSystemManager.Instance.HeroSaveSystem.Load();
            var settings = PlayerData.HeroSettingsContainer;

            var playerHeroes = new Hero.Hero[selectedHeroIds.Length];
            for (int i = 0; i < selectedHeroIds.Length; i++)
            {
                var heroData = FindHeroDataByID(allPlayerHeroesDatas, selectedHeroIds[i]);
                settings.TryGetHeroSettings(heroData.ID, out var heroSettings);
                playerHeroes[i] = new Hero.Hero(heroSettings, heroData);
            }

            var enemyHeroes = new Hero.Hero[2];
            for (int i = 0; i < enemyHeroes.Length; i++)
            {
                enemyHeroes[i] = heroFactory.CreateRandomHero(settings, HeroTeam.Enemy, false);
            }

            var levelData = new LevelData(
                CreateGameHeroDatas(playerHeroes),
                CreateGameHeroDatas(enemyHeroes));

            var response = new CreateLevelDataResponse();
            response.IsSuccessfull = true;
            response.LevelData = levelData;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        private GameHeroData[] CreateGameHeroDatas(Hero.Hero[] heroes)
        {
            var gameHeroesDatas = new GameHeroData[heroes.Length];
            for (int i = 0; i < heroes.Length; i++)
            {
                gameHeroesDatas[i] = new GameHeroData(heroes[i]);
            }
            _mockHeroes.Add(heroes[0].HeroTeam, gameHeroesDatas);

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

            return Task.FromResult(JsonUtility.ToJson(response));
        }
    }
}
