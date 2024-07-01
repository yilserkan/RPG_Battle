using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Game
{
    public class GameHeroAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private readonly int IDLE_ANIM_HASH = Animator.StringToHash("Idle");
        private readonly int ATTACK_ANIM_HASH = Animator.StringToHash("Attack");
        private readonly int DEATH_ANIM_HASH = Animator.StringToHash("Death");
        private readonly int TAKEDAMAGE_ANIM_HASH = Animator.StringToHash("TakeDamage");

        private Dictionary<AnimationType, int> _animationHashDict;

        public void SetupAnimator(AnimatorOverrideController overrideController)
        {
            _animationHashDict = new Dictionary<AnimationType, int>()
            {
                { AnimationType.Idle, IDLE_ANIM_HASH },
                { AnimationType.Attack, ATTACK_ANIM_HASH},
                { AnimationType.Death, DEATH_ANIM_HASH},
                { AnimationType.TakeDamage, TAKEDAMAGE_ANIM_HASH}
            };

            _animator.runtimeAnimatorController = overrideController;
        }

        public void PlayAnimation(AnimationType type)
        {
             if(!_animationHashDict.TryGetValue(type, out var anim)) return;

            _animator.Play(anim);
        }

        public enum AnimationType
        {
            Idle,
            Attack,
            Death,
            TakeDamage
        }

    }
}

