using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Stats;
using RPGGame.Pool;

namespace RPGGame.Game
{
    public class GameHero : Pool.Poolable
    {
        [SerializeField] private GameHeroAnimationController _animationController;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private float _vitality;
        private Hero.Hero _hero;
        private HeroTeam _team;

        public Hero.Hero Hero => _hero;
        public float Vitality => _vitality;
        public HeroTeam Team => _team;

        public void Initialize(Hero.Hero hero)
        {
            _team = (HeroTeam)hero.HeroTeam;
            _hero = hero;
            _vitality = _hero.Stats.CalculateAttributeValue(StatConstants.Vitality);
            _animationController.SetupAnimator(hero.Settings.AnimatorOverrideController);
            _animationController.PlayAnimation(GameHeroAnimationController.AnimationType.Idle);
            SetSpriteOrientation();
        }

        private void SetSpriteOrientation()
        {
            _spriteRenderer.flipX = _team == HeroTeam.Enemy;
        }
    
        public void Attack(GameHero gameHero)
        {
            Debug.LogWarning($"{Hero.Settings.Name} attacked {gameHero.Hero.Settings.Name}");
        }

    }

    public enum HeroTeam
    {
        Player,
        Enemy
    }
}
