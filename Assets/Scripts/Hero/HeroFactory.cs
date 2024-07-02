using RPGGame.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Hero
{
    public class HeroFactory : IHeroFactory
    {
        public Hero Create(HeroSettingsContainer heroSettingsContainer, HeroData heroData)
        {
            if(!heroSettingsContainer.TryGetHeroSettings(heroData.ID, out var settings)) { return null; }

            return new Hero(settings, heroData);
        }


        public Hero CreateRandomHero(HeroSettingsContainer heroSettingsContainer, HeroTeam team, bool removeObtainedHeroes = true)
        {
            var availableHeroes = heroSettingsContainer.GetAvailableHeroes(removeObtainedHeroes);
            if(availableHeroes == null || availableHeroes.Length == 0)
                return null;

            var randomIndex = Random.Range(0, availableHeroes.Length);
            var heroSettings = availableHeroes[randomIndex];
            var heroData = new HeroData(heroSettings.ID, team);
            var hero = new Hero(heroSettings, heroData);
            return hero;
        }
    }

    public interface IHeroFactory
    {
        Hero Create(HeroSettingsContainer heroSettingsContainer, HeroData heroData);
        Hero CreateRandomHero(HeroSettingsContainer heroSettingsContainer, HeroTeam team, bool removeObtainedHeroes = true);
    }
}

