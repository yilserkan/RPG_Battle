using RPGGame.Pool;
using RPGGame.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Hero
{
    public class GameHeroFactory : IGameHeroFactory
    {
        private ObjectPoolSettings _heroFlyweightSettings;
        private Transform _parent;

        public GameHeroFactory(ObjectPoolSettings flyweightSettings, Transform parent = null)
        {
            _heroFlyweightSettings = flyweightSettings;
            _parent = parent;
        }

        public GameHero Create(HeroSettingsContainer heroContainerSettings, GameHeroData heroData)
        {
            var heroGameObject = ObjectPool.Spawn(_heroFlyweightSettings, _parent);
            if (!heroGameObject.TryGetComponent<GameHero>(out var gameHero)) { return null; }
            if (!heroContainerSettings.TryGetHeroSettings(heroData.ID, out var heroSettings)) { return null; }

            var newHero = new Hero(heroSettings, heroData);
            gameHero.Initialize(newHero, heroData);

            return gameHero;
        }
    }
}
