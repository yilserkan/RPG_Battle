using RPGGame.Game;
using UnityEngine;

namespace RPGGame.Skills
{
    [CreateAssetMenu(menuName ="ScriptableOjects/Skills/BasicSkill")]
    public class BasicSkillSettings : AbstractSkillSettings
    {
        private GameHero _attacker;
        private GameHero _receiver;

        public override void ExecuteSkill(GameHero attacker, GameHero receiver)
        {
            _attacker = attacker;  
            _receiver = receiver;
            AddListeners();
            attacker.AnimationController.AnimatePlayerToTarget(receiver);
            attacker.AnimationController.PlayAnimation(AnimationName);
        }

        private void OnAttackAnimationPlayer()
        {
            Debug.LogWarning("Attacking..");
            _receiver.AnimationController.PlayAnimation(GameHeroAnimationController.AnimationType.TakeDamage);
        }

        private void OnAttackAnimationCompleted()
        {
            Debug.LogWarning("Attacking Completed " + _attacker.Hero.Settings.Name);
            RemoveListeners();
            _attacker.AnimationController.AnimatePlayerToStartPoint();
            _attacker.OnAttackCompleted();
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
