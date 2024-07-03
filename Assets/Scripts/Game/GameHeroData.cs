using RPGGame.Hero;
using RPGGame.Player;
using RPGGame.Stats;
using System;

namespace RPGGame.Game
{
    [Serializable]
    public class GameHeroData : HeroData
    {
        public float RemainingVitality;

        public GameHeroData(HeroData herodata) : base(herodata)
        {
            RemainingVitality = GetHeroInitialVitality(herodata);
        }

        public GameHeroData(GameHero gameHero) : base(gameHero.Hero)
        {
            RemainingVitality = gameHero.HealthController.Vitality;
        }

        private float GetHeroInitialVitality(HeroData gameHeroData)
        {
            var heroSettingsContainer = PlayerData.HeroSettingsContainer;
            if (!heroSettingsContainer.TryGetHeroSettings(gameHeroData.ID, out var settings)) return 0;

            var stats = new CharacterStat(settings.BaseStats, gameHeroData.Level);
            return stats.CalculateAttributeValue(StatConstants.Vitality);
        }
    }
}

