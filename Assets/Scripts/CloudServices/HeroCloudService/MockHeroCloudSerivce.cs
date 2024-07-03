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
        private const int START_HERO_COUNT = 3;
        private IHeroFactory heroFactory = new HeroFactory();

        public Task<string> AddRandomHeroToPlayer()
        {
            var heroSettingsContainer = PlayerData.HeroSettingsContainer;
            var hero = heroFactory.CreateRandomHeroData(heroSettingsContainer, HeroTeam.Player);

            var heroSaveSystem = SaveSystemManager.Instance.HeroSaveSystem;

            var playerHeroes = heroSaveSystem.Load().ToList();
            playerHeroes.Add(hero);
            var newPlayerHeroes = playerHeroes.ToArray();

            heroSaveSystem.Save(newPlayerHeroes);

            var response = new BaseResponse();
            response.IsSuccessfull = true;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        public Task<string> LoadHeroData()
        {
            var heroSaveSystem = SaveSystemManager.Instance.HeroSaveSystem;

            HeroData[] heroDatas;
            if(heroSaveSystem.HasSaveFile())
            {
                heroDatas = heroSaveSystem.Load();
            }
            else
            {
                heroDatas = CreateInitialRandomHeroes();
                heroSaveSystem.Save(heroDatas);
            }

            var response = new LoadHeroDataResponse();
            response.IsSuccessfull = true;
            response.HeroDatas = heroDatas;

            return Task.FromResult(JsonUtility.ToJson(response));
        }

        private HeroData[] CreateInitialRandomHeroes()
        {
            var heroSettingsContainer = PlayerData.HeroSettingsContainer;
            var intialPlayerHeroes = new HeroData[START_HERO_COUNT];
            for (int i = 0; i < START_HERO_COUNT; i++)
            {
                var hero = heroFactory.CreateRandomHeroData(heroSettingsContainer, HeroTeam.Player);
                intialPlayerHeroes[i] = hero;
            }

            return intialPlayerHeroes;
        }
    }
}

