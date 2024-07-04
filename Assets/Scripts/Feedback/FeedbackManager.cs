using RPGGame.Game;
using RPGGame.HeroSelection;
using RPGGame.Pool;
using RPGGame.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Feedback
{
    public class FeedbackManager : MonoBehaviour
    {
        [SerializeField] private ObjectPoolSettings _poolSettings;
        [SerializeField] private Transform _feedbackParent;

        private void OnEnable()
        {
            AddListeners();   
        }

        private void OnDisable()
        {
            RemoveListeners();
        }
   
        private void HandleOnRequestFeedback(FeedbackData feedbackData)
        {
            var pooled = ObjectPool.Spawn(_poolSettings, _feedbackParent);
            if (!pooled.TryGetComponent(out Feedback feedback)) { return; }
            feedback.Initialize(feedbackData);
        }

        private void AddListeners()
        {
            GameHeroHealthController.RequestTakeDamageFeedbackEvent += HandleOnRequestFeedback;
            LevelUpFeedbackController.RequestLevelUpFeedback += HandleOnRequestFeedback;
            HeroSelectionManager.RequestSelectHeroesFeedbackEvent += HandleOnRequestFeedback;
        }

        private void RemoveListeners()
        {
            GameHeroHealthController.RequestTakeDamageFeedbackEvent -= HandleOnRequestFeedback;
            LevelUpFeedbackController.RequestLevelUpFeedback -= HandleOnRequestFeedback;
            HeroSelectionManager.RequestSelectHeroesFeedbackEvent -= HandleOnRequestFeedback;
        }
    }

    public struct FeedbackData
    {
        public string Text;
        public Color Color;
        public float Duration;
        public Vector2 Position;
        public FeedbackPositionType PositionType;
    }

    public enum FeedbackPositionType
    {
        Position, 
        AnchoredPostiion
    }
}

