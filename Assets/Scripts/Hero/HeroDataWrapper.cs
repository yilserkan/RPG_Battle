using System;
using System.Collections.Generic;

namespace RPGGame.Hero
{
    [Serializable]
    public class HeroDataWrapper
    {
        public HeroData[] HeroDatas;

        public HeroDataWrapper(List<Hero> heroes)
        {
            HeroDatas = new HeroData[heroes.Count];
            for (int i = 0; i < heroes.Count; i++)
            {
                HeroDatas[i] = new HeroData(heroes[i]);
            }
        }
    }
}

