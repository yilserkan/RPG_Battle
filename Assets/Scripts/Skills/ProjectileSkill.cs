using UnityEngine;
using DG.Tweening;
using RPGGame.Pool;

namespace RPGGame.Skills
{
    public class ProjectileSkill : BaseSkill
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector3 _spawnOffset;

        public override void ExecuteSkill()
        {
            var projectileSettings = (ProjectileSkillSettings)_skillSettings;
            _animator.runtimeAnimatorController = projectileSettings.SkillAnimatorOverrideController;
            
            transform.position = projectileSettings.Attacker.SpawnPoint.GetPosition() + _spawnOffset;

            var dir = projectileSettings.Receiver.transform.position - projectileSettings.Attacker.transform.position;
            transform.right = dir;

            AnimateToTarget();
        }

        private void AnimateToTarget()
        {
            var projectileSettings = (ProjectileSkillSettings)_skillSettings;

            transform.DOMove(_skillSettings.Receiver.SpawnPoint.GetPosition(), projectileSettings.ProjectileDuration).
                OnComplete(() => OnSkillCompleted());
        }

        public void OnSkillCompleted()
        {
            var projectileSettings = (ProjectileSkillSettings)_skillSettings;
            projectileSettings.OnAttackAnimationCompleted();
            ObjectPool.ReturnToPool(this);
        }
    }
}

