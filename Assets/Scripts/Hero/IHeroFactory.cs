using RPGGame.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Hero
{
    public interface IHeroFactory
    {
        Hero CreateHero(HeroData heroData);
        Hero CreateRandomHero(HeroTeam team, HeroData[] heroesToIgnore = null);
        HeroData CreateRandomHeroData(HeroTeam team, HeroData[] heroesToIgnore = null, int startLevel = 1);

    }
}
