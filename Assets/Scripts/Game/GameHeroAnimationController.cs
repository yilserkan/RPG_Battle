using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Game
{
    public class GameHeroAnimationController : MonoBehaviour
    {
        [SerializeField] private GameHero _gameHero;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector3 _attackerOffsetToTarget;

        private readonly int IDLE_ANIM_HASH = Animator.StringToHash("Idle");
        private readonly int ATTACK_ANIM_HASH = Animator.StringToHash("Attack");
        private readonly int DEATH_ANIM_HASH = Animator.StringToHash("Death");
        private readonly int TAKEDAMAGE_ANIM_HASH = Animator.StringToHash("TakeDamage");

        private Dictionary<AnimationType, int> _animationHashDict;

        public event Action OnPlayerPlayedAttackAnimationEvent;
        public event Action OnPlayerCompletedAttackAnimationEvent;

        private const float _playerMoveDuration = .1f;

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

            //_animator.
        }

        public void PlayAnimation(string animationName)
        {
            var animHash = Animator.StringToHash(animationName);
            _animator.Play(animHash);
        }

        public void AnimatePlayerToTarget(GameHero receiver)
        {
            AnimatePlayerToPosition(receiver.SpawnPoint.GetPosition() + GetTargetOffset());
        }

        private Vector3 GetTargetOffset()
        {
            return _gameHero.Team == HeroTeam.Player ? _attackerOffsetToTarget : -_attackerOffsetToTarget;
        }

        public void AnimatePlayerToStartPoint()
        {
            AnimatePlayerToPosition(_gameHero.SpawnPoint.GetPosition());
        }

        private void AnimatePlayerToPosition(Vector3 targetPos)
        {
            transform.DOMove(targetPos, _playerMoveDuration);
        }

        public void AnimEvent_OnPlayerAttacked()
        {
            OnPlayerPlayedAttackAnimationEvent?.Invoke();
        }

        public void AnimEvent_OnAttackAnimationCompleted()
        {
            OnPlayerCompletedAttackAnimationEvent?.Invoke();
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

