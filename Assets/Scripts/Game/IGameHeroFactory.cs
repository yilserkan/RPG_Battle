using RPGGame.Game;

namespace RPGGame.Hero
{
    public interface IGameHeroFactory
    {
        GameHero Create(HeroSettingsContainer heroContainerSettings, GameHeroData heroData = null);
    }
}