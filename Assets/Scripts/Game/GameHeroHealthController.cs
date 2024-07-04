using RPGGame.Feedback;
using RPGGame.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.Game
{
    public class GameHeroHealthController : MonoBehaviour
    {
        [SerializeField] private GameHero _gameHero;
        [SerializeField] private Image _healthBarImage;

        private float _vitality;
        private float _maxVitality;
        private bool _isDead;

        public bool IsDead => _isDead;
        public float Vitality => _vitality;

        public static event Action<FeedbackData> RequestTakeDamageFeedbackEvent;

        public void Initialize()
        {
            SetVitality();
            CheckIfHeroDied();
        }

        private void SetVitality()
        {
            _vitality = _gameHero.HeroData.RemainingVitality; ;
            _maxVitality = _gameHero.Hero.Stats.GetAttributeValue(StatTypes.Vitality);
            UpdateHealthUI();
        }

        public void TakeDamage(float damage)
        {
            _vitality -= damage;
            CheckIfHeroDied();
            UpdateHealthUI();
            RequestTakeDamageFeedback(damage);
        }

        private void CheckIfHeroDied()
        {
            _isDead = Vitality <= 0;
            if (_isDead)
            {
                Die();
            }
        }

        private void RequestTakeDamageFeedback(float damage)
        {
            var pos = Camera.main.WorldToScreenPoint(_gameHero.SpawnPoint.GetPosition());
            var feedbackData = new FeedbackData()
            {
                Text = $"-{(int)damage}",
                Position = pos ,
                PositionType = FeedbackPositionType.Position,
                Color = Color.red,
                Duration = 1
            };

            RequestTakeDamageFeedbackEvent?.Invoke(feedbackData);
        }

        private void Die()
        {
            _vitality = 0;
            _gameHero.AnimationController.PlayAnimation(GameHeroAnimationController.AnimationType.Death);
        }

        private void UpdateHealthUI()
        {
            _healthBarImage.fillAmount = _vitality / _maxVitality;
        }
    }
}
