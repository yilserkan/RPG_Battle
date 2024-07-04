using RPGGame.Config;
using RPGGame.Game;
using RPGGame.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPGGame.Game
{
    public class GameHeroLevelController : MonoBehaviour
    {
        [SerializeField] private GameHero _gameHero;
        [SerializeField] private TextMeshProUGUI _levelText;

        public static event Action<LevelUpData> OnPlayerLeveldUp;

        public void Initialize()
        {
            _levelText.text = $"{_gameHero.Hero.Level}";
        }

        public void LocallyIncreaseExperience()
        {
            if (_gameHero.HealthController.IsDead) return;

            var playerdata = _gameHero.Hero;
            playerdata.Experience++;

            if (playerdata.Experience % GameConfig.Data.LevelIncreaseInterval == 0)
            {
                playerdata.Level++;
                var modifiedAttributes = playerdata.Stats.HandleOnPlayerLeveldUp();
                var levelUpData = new LevelUpData() { HeroName = _gameHero.Hero.Settings.Name, ModifiedAttributes = modifiedAttributes };
                OnPlayerLeveldUp?.Invoke(levelUpData);
            }
        }
    }

    public struct LevelUpData
    {
        public string HeroName;
        public AttributeModifiedData[] ModifiedAttributes;
    }
}



