using RPGGame.Game;
using RPGGame.Stats;
using UnityEngine;

namespace RPGGame.Skills
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Skills/BasicSkill")]
    public class BasicSkillSettings : AbstractSkillSettings
    {
        private float _damage;

        public override void ExecuteSkill(GameHero attacker, GameHero receiver, float damage)
        {
            _damage = damage;
            _attacker = attacker;  
            _receiver = receiver;
            AddListeners();
            attacker.AnimationController.AnimatePlayerToTarget(receiver);
            attacker.AnimationController.PlayAnimation(AnimationName);
        }

        private void OnAttackAnimationPlayer()
        {
            _receiver.AnimationController.PlayAnimation(GameHeroAnimationController.AnimationType.TakeDamage);
            _receiver.HealthController.TakeDamage(_damage);
        }

        private void OnAttackAnimationCompleted()
        {
            RemoveListeners();
            _attacker.AnimationController.AnimatePlayerToStartPoint();
            _attacker.SkillController.OnAttackCompleted();
        }

        private void AddListeners()
        {
            var attackerAnimController = _attacker.AnimationController;
            attackerAnimController.OnPlayerPlayedAttackAnimationEvent += OnAttackAnimationPlayer;
            attackerAnimController.OnPlayerCompletedAttackAnimationEvent += OnAttackAnimationCompleted;
        }

        private void RemoveListeners()
        {
            var attackerAnimController = _attacker.AnimationController;
            attackerAnimController.OnPlayerPlayedAttackAnimationEvent -= OnAttackAnimationPlayer;
            attackerAnimController.OnPlayerCompletedAttackAnimationEvent -= OnAttackAnimationCompleted;
        }
    }
}
