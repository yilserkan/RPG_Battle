using RPGGame.Hero;
using RPGGame.Stats;
using System;

namespace RPGGame.Game
{
    [Serializable]
    public class GameHeroData : HeroData
    {
        public float RemainingVitality;

        public GameHeroData(string id, HeroTeam team) : base(id, team)
        {
        }

        public GameHeroData(Hero.Hero hero) : base(hero)
        {
            RemainingVitality = hero.Stats.CalculateAttributeValue(StatConstants.Vitality);
        }

        public GameHeroData(GameHero gameHero) : base(gameHero.Hero)
        {
            RemainingVitality = gameHero.HealthController.Vitality;
        }
    }
}

