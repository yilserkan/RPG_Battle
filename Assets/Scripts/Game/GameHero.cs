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
        [SerializeField] private GameHeroHealthController _healthController;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Hero.Hero _hero;
        private HeroTeam _team;
        private SpawnPoint _spawnPoint;
        private GameHeroSkillController _skillController;

        public Hero.Hero Hero => _hero;
        public HeroTeam Team => _team;
        public SpawnPoint SpawnPoint => _spawnPoint;

        public GameHeroAnimationController AnimationController => _animationController;
        public GameHeroHealthController HealthController => _healthController;
        public GameHeroSkillController SkillController => _skillController;

        public void Initialize(Hero.Hero hero)
        {
            _skillController = new GameHeroSkillController(this);

            _team = (HeroTeam)hero.HeroTeam;
            _hero = hero;
          
            _animationController.SetupAnimator(hero.Settings.AnimatorOverrideController);
            _animationController.PlayAnimation(GameHeroAnimationController.AnimationType.Idle);

            HealthController.Initialize();

            SetSpriteOrientation();
        }

        public void SetSpawnPoint(SpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
            transform.position = spawnPoint.GetPosition();
        }

        private void SetSpriteOrientation()
        {
            var flipX = _team == HeroTeam.Enemy;
            _spriteRenderer.flipX = flipX;
        }

        public float GetHeroStat(string statId)
        {
            return _hero.Stats.CalculateAttributeValue(statId);
        }

        public void ResetHero()
        {
            SpawnPoint.SetOccupied(false);
            _spawnPoint = null;
            ObjectPool.ReturnToPool(this);
        }
    }

    public enum HeroTeam
    {
        Player,
        Enemy
    }
}
