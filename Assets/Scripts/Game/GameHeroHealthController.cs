using RPGGame.Stats;
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

        private void Start()
        {
            _levelText.text = $"{_gameHero.Hero.Level}";
            SetVitality();
            _isDead = false;
        }

        private void SetVitality()
        {
            var startVitality = _gameHero.GetHeroStat(StatConstants.Vitality);
            _vitality = startVitality;
            _maxVitality = startVitality;
            UpdateHealthUI();
        }

        public void TakeDamage(float damage)
        {
            _vitality -= damage;
            if (_vitality <= 0)
            {
                Die();
            }
            UpdateHealthUI();
        }

        private void Die()
        {
            _vitality = 0;
            _isDead = true;
            _gameHero.AnimationController.PlayAnimation(GameHeroAnimationController.AnimationType.Death);
        }

        private void UpdateHealthUI()
        {
            _healthBarImage.fillAmount = _vitality / _maxVitality;
        }
    }
}
