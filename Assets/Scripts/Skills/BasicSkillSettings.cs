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
            Debug.LogWarning("Attacking..");
            _receiver.AnimationController.PlayAnimation(GameHeroAnimationController.AnimationType.TakeDamage);
            _receiver.HealthController.TakeDamage(_damage);
        }

        private void OnAttackAnimationCompleted()
        {
            Debug.LogWarning("Attacking Completed " + _attacker.Hero.Settings.Name);
            RemoveListeners();
            _attacker.AnimationController.AnimatePlayerToStartPoint();
            _attacker.SkillController.OnAttackCompleted();
        }

        private float CalculateDamage()
        {
            var attackerDamage = Attacker.GetHeroStat(StatConstants.Attack);
            var attackerCrit = Attacker.GetHeroStat(StatConstants.Critical);

            var isCriticalHit = Random.Range(0, 100) < 10;

            if (isCriticalHit)
                attackerDamage += attackerCrit;

            return attackerDamage;
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
