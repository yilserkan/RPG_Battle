using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Stats;
using RPGGame.Pool;
using RPGGame.Level;
using System;

namespace RPGGame.Game
{
    public class GameHero : Pool.Poolable
    {
        [SerializeField] private GameHeroAnimationController _animationController;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private float _vitality;
        private Hero.Hero _hero;
        private HeroTeam _team;
        private SpawnPoint _spawnPoint;

        public Hero.Hero Hero => _hero;
        public float Vitality => _vitality;
        public HeroTeam Team => _team;
        public SpawnPoint SpawnPoint => _spawnPoint;
        public GameHeroAnimationController AnimationController => _animationController;

        public static event Action OnHeroCompletedAttack;

        public void Initialize(Hero.Hero hero)
        {
            _team = (HeroTeam)hero.HeroTeam;
            _hero = hero;
            _vitality = _hero.Stats.CalculateAttributeValue(StatConstants.Vitality);
            _animationController.SetupAnimator(hero.Settings.AnimatorOverrideController);
            _animationController.PlayAnimation(GameHeroAnimationController.AnimationType.Idle);
            SetSpriteOrientation();
        }

        public void SetSpawnPoint(SpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
            transform.position = spawnPoint.GetPosition();
        }

        private void SetSpriteOrientation()
        {
            _spriteRenderer.flipX = _team == HeroTeam.Enemy;
        }
    
        public void Attack(GameHero gameHero)
        {
            Debug.LogWarning($"{Hero.Settings.Name} attacked {gameHero.Hero.Settings.Name}");
            Hero.Settings.Skill.ExecuteSkill(this, gameHero);
        }

        public void OnAttackCompleted()
        {
            OnHeroCompletedAttack?.Invoke();
        }

    }

    public enum HeroTeam
    {
        Player,
        Enemy
    }
}
