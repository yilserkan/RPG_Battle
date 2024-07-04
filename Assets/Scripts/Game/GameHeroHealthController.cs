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
        [SerializeField] private TextMeshProUGUI _levelText;

        private float _vitality;
        private float _maxVitality;
        private bool _isDead;

        public bool IsDead => _isDead;
        public float Vitality => _vitality;

        public static event Action<FeedbackData> RequestTakeDamageFeedbackEvent;

        public void Initialize()
        {
            _levelText.text = $"{_gameHero.Hero.Level}";
            SetVitality();
            CheckIfHeroDied();
        }

        private void SetVitality()
        {
            var startVitality = _gameHero.HeroData.RemainingVitality;
            _vitality = startVitality;
            _maxVitality = _gameHero.Hero.Stats.CalculateAttributeValue(StatConstants.Vitality);
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
            var data = FeedbackManager.CreateFeedbackData(
                $"-{damage:f2}",
                pos,
                1,
                Color.red
                );
            RequestTakeDamageFeedbackEvent?.Invoke( data );
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
