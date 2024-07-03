using RPGGame.Game;
using RPGGame.Player;
using System.Collections;
using System.Collections.Generic;
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


        public Hero CreateRandomHero(HeroSettingsContainer heroSettingsContainer, HeroTeam team, bool removeObtainedHeroes = true)
        {
            var heroData = CreateRandomHeroData(heroSettingsContainer, team, removeObtainedHeroes);
            if(!heroSettingsContainer.TryGetHeroSettings(heroData.ID, out var settings)) return null;
            var hero = new Hero(settings, heroData);
            return hero;
        }

        public HeroData CreateRandomHeroData(HeroSettingsContainer heroSettingsContainer, HeroTeam team, bool removeObtainedHeroes = true)
        {
            var availableHeroes = heroSettingsContainer.GetAvailableHeroes(removeObtainedHeroes);
            if (availableHeroes == null || availableHeroes.Length == 0)
                return null;

            var randomIndex = Random.Range(0, availableHeroes.Length);
            var heroSettings = availableHeroes[randomIndex];
            var heroData = new HeroData(heroSettings.ID, team);
            return heroData;
        }
    }

    public interface IHeroFactory
    {
        Hero CreateHero(HeroData heroData);
        Hero CreateRandomHero(HeroSettingsContainer heroSettingsContainer, HeroTeam team, bool removeObtainedHeroes = true);
        HeroData CreateRandomHeroData(HeroSettingsContainer heroSettingsContainer, HeroTeam team, bool removeObtainedHeroes = true);
        
    }
}

