using RPGGame.Game;
using RPGGame.Player;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGGame.Hero
{
    public class HeroFactory : IHeroFactory
    {
        public Hero CreateHero(HeroData heroData)
        {
            var heroSettingsContainer = PlayerData.HeroSettingsContainer;

            if (!heroSettingsContainer.TryGetHeroSettings(heroData.ID, out var settings)) { return null; }

            return new Hero(settings, heroData);
        }


        public Hero CreateRandomHero(HeroTeam team, HeroData[] heroesToIgnore = null)
        {
            var heroSettingsContainer = PlayerData.HeroSettingsContainer;
            var heroData = CreateRandomHeroData(team, heroesToIgnore);
            if(!heroSettingsContainer.TryGetHeroSettings(heroData.ID, out var settings)) return null;
            var hero = new Hero(settings, heroData);
            return hero;
        }

        public HeroData CreateRandomHeroData(HeroTeam team, HeroData[] heroesToIgnore = null, int startLevel = 1)
        {
            var availableHeroes = GetAvailableHeroes(heroesToIgnore);
            if (availableHeroes == null || availableHeroes.Length == 0)
                return null;

            var randomIndex = Random.Range(0, availableHeroes.Length);
            var heroSettings = availableHeroes[randomIndex];
            var heroData = new HeroData(heroSettings.ID, team, startLevel);

            return heroData;
        }

        public HeroSettings[] GetAvailableHeroes(HeroData[] heroesToIgnore = null)
        {
            var heroSettingsContainer = PlayerData.HeroSettingsContainer;
            var availableHeroes = new Dictionary<string, HeroSettings>(heroSettingsContainer.HeroSettingsDict);

            if (heroesToIgnore != null)
            {
                for (int i = 0; i < heroesToIgnore.Length; i++)
                {
                    if (heroesToIgnore[i] == null) continue;
                    availableHeroes.Remove(heroesToIgnore[i].ID);
                }
            }

            return availableHeroes.Values.ToArray();
        }
    }

    public interface IHeroFactory
    {
        Hero CreateHero(HeroData heroData);
        Hero CreateRandomHero(HeroTeam team, HeroData[] heroesToIgnore = null);
        HeroData CreateRandomHeroData(HeroTeam team, HeroData[] heroesToIgnore = null, int startLevel = 1);
        
    }
}

