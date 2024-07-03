using RPGGame.Skills;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Game
{
    public class GameHeroSkillController
    {
        private GameHero _gameHero;
        public static event Action OnHeroCompletedAttack;

        private AbstractSkillSettings _skill => _gameHero.Hero.Settings.Skill;

        public GameHeroSkillController(GameHero gameHero)
        {
            _gameHero = gameHero;
        }

        public void Attack(GameHero receiver, float damage)
        {
            Debug.LogWarning($"{_gameHero.Hero.Settings.Name} attacked {receiver.Hero.Settings.Name}");
            _skill.ExecuteSkill(_gameHero, receiver, damage);
        }

        public void OnAttackCompleted()
        {
            OnHeroCompletedAttack?.Invoke();
        }
    }
}
