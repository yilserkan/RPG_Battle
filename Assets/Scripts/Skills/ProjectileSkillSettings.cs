using RPGGame.Game;
using RPGGame.Pool;
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
        public Vector3 Dir;

        public override void ExecuteSkill(GameHero attacker, GameHero receiver)
        {
            _attacker = attacker;
            _receiver = receiver;
            AddListeners();
            attacker.AnimationController.PlayAnimation(AnimationName);
        }

        private void OnAttackAnimationPlayer()
        {
            Debug.LogWarning("Attacking..");

            var pooled = ObjectPool.Spawn(PoolSettings, _attacker.transform);
            if (pooled.TryGetComponent(out BaseSkill skill))
            {
                skill.Initialize(this);
            }
        }

        public void OnAttackAnimationCompleted()
        {
            Debug.LogWarning("Attacking Completed " + _attacker.Hero.Settings.Name);
            RemoveListeners();
            _receiver.AnimationController.PlayAnimation(GameHeroAnimationController.AnimationType.TakeDamage);
            _attacker.OnAttackCompleted();
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

