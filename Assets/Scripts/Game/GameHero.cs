using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Pool;
using RPGGame.Level;
using System;


namespace RPGGame.Game
{
    public class GameHero : Pool.Poolable
    {
        [SerializeField] private GameHeroAnimationController _animationController;
        [SerializeField] private GameHeroHealthController _healthController;
        [SerializeField] private GameHeroLevelController _levelController;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private HeroTeam _team;
        private Hero.Hero _hero;
        private GameHeroData _heroData;
        private SpawnPoint _spawnPoint;
        private GameHeroSkillController _skillController;

        public HeroTeam Team => _team;
        public Hero.Hero Hero => _hero;
        public GameHeroData HeroData => _heroData;
        public SpawnPoint SpawnPoint => _spawnPoint;
        public GameHeroAnimationController AnimationController => _animationController;
        public GameHeroHealthController HealthController => _healthController;
        public GameHeroLevelController LevelController => _levelController;
        public GameHeroSkillController SkillController => _skillController;

        public void Initialize(Hero.Hero hero, GameHeroData gameHeroData)
        {
            _skillController = new GameHeroSkillController(this);

            _team = (HeroTeam)hero.HeroTeam;
            _hero = hero;
            _heroData = gameHeroData;

            _animationController.SetupAnimator(hero.Settings.AnimatorOverrideController);
            _animationController.PlayAnimation(GameHeroAnimationController.AnimationType.Idle);

            HealthController.Initialize();
            LevelController.Initialize();

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
