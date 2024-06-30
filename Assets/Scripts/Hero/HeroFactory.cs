using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Hero
{
    public class HeroFactory : IHeroFactory
    {
        public Hero Create(HeroSettings heroSettings, HeroData heroData = null)
        {
            return new Hero(heroSettings, heroData);
        }
    }

    public interface IHeroFactory
    {
        Hero Create(HeroSettings heroSettings, HeroData heroData = null);
    }
}

