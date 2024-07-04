using RPGGame.Config;
using RPGGame.Game;
using RPGGame.Hero;
using RPGGame.Player;
using RPGGame.SaveSystem;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.CloudServices
{
    public class MockHeroCloudSerivce : IHeroCloudService
    {
        private IHeroFactory heroFactory = new HeroFactory();
        private HeroSaveSystem _heroSaveSystem = new HeroSaveSystem();

        public Task<string> AddRandomHeroToPlayer()
        {
            var playerHeroes = _heroSaveSystem.Load();
            var hero = heroFactory.CreateRandomHeroData(HeroTeam.Player, playerHeroes);

            var response = new BaseResponse();

            if (hero == null)
            {
                response.IsSuccessfull = false;
                return Task.FromResult(JsonUtility.ToJson(response));
            }

            var playerHeroesList = playerHeroes.ToList();
            playerHeroesList.Add(hero);
            var newPlayerHeroes = playerHeroesList.ToArray();

            _heroSaveSystem.Save(newPlayerHeroes);

           
            response.IsSuccessfull = true;
            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task<string> IncreaseHeroExperiences(string heroIdsJson)
        {
            var requestData = JsonUtility.FromJson<IncreaseHeroExperienceRequest>(heroIdsJson);
            var heroIds = requestData.HeroIds;
            var playerHeroDatas = _heroSaveSystem.Load();

            for (int i = 0; i < heroIds.Length; i++)
            {
                for (int j = 0; j < playerHeroDatas.Length; j++)
                {
                    if (heroIds[i] == playerHeroDatas[j].ID)
                    {
                        playerHeroDatas[j].Experience++;

                        bool hasLeveldUp = playerHeroDatas[j].Experience % GameConfig.Data.LevelIncreaseInterval == 0;
                        if (hasLeveldUp)
                        {
                            playerHeroDatas[j].Level++;
                        }

                        break;
                    }
                }
            }

            _heroSaveSystem.Save(playerHeroDatas);

            var response = new BaseResponse();
            response.IsSuccessfull = true;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task<string> LoadHeroData()
        {
            HeroData[] heroDatas;
            if(_heroSaveSystem.HasSaveFile())
            {
                heroDatas = _heroSaveSystem.Load();
            }
            else
            {
                heroDatas = CreateInitialRandomHeroes();
                _heroSaveSystem.Save(heroDatas);
            }

            var response = new LoadHeroDataResponse();
            response.IsSuccessfull = true;
            response.HeroDatas = heroDatas;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        private HeroData[] CreateInitialRandomHeroes()
        {
            var initialHeroCount = GameConfig.Data.StartHeroCount;
            var intialPlayerHeroes = new HeroData[initialHeroCount];
            for (int i = 0; i < initialHeroCount; i++)
            {
                var hero = heroFactory.CreateRandomHeroData(HeroTeam.Player, intialPlayerHeroes);
                intialPlayerHeroes[i] = hero;
            }

            return intialPlayerHeroes;
        }
    }
}

