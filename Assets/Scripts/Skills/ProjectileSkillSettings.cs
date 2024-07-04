using RPGGame.Game;
using RPGGame.Pool;
using RPGGame.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Skills
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Skills/ProjectileSkill")]
    public class ProjectileSkillSettings : AbstractSkillSettings
    {
        public ObjectPoolSettings PoolSettings;
        public AnimatorOverrideController SkillAnimatorOverrideController;
        public float ProjectileDuration;
        private float _damage;

        public override void ExecuteSkill(GameHero attacker, GameHero receiver, float damage)
        {
            _damage = damage;
            _attacker = attacker;
            _receiver = receiver;
            AddListeners();
            attacker.AnimationController.PlayAnimation(AnimationName);
        }

        private void OnAttackAnimationPlayer()
        {
            var pooled = ObjectPool.Spawn(PoolSettings, _attacker.transform);
            if (pooled.TryGetComponent(out BaseSkill skill))
            {
                skill.Initialize(this);
            }
        }

        public void OnAttackAnimationCompleted()
        {
            RemoveListeners();
            _receiver.AnimationController.PlayAnimation(GameHeroAnimationController.AnimationType.TakeDamage);
            _receiver.HealthController.TakeDamage(_damage);
            _attacker.SkillController.OnAttackCompleted();
        }

        private void AddListeners()
        {
            var attackerAnimController = _attacker.AnimationController;
            attackerAnimController.OnPlayerPlayedAttackAnimationEvent += OnAttackAnimationPlayer;
        }

        private void RemoveListeners()
        {
            var attackerAnimController = _attacker.AnimationController;
            attackerAnimController.OnPlayerPlayedAttackAnimationEvent -= OnAttackAnimationPlayer;
        }
    }
}

