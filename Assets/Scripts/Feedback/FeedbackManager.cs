using RPGGame.Game;
using RPGGame.Pool;
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
        public static FeedbackData CreateFeedbackData(string text, Vector2 position = default, float duration = 1, Color color = default)
        {
            var data = new FeedbackData();
            data.Text = text;
            data.Position = position == default ? Vector2.zero : position;
            data.Duration = duration;
            data.Color = color == default ? Color.white : color;

            return data;
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
        }

        private void RemoveListeners()
        {
            GameHeroHealthController.RequestTakeDamageFeedbackEvent -= HandleOnRequestFeedback;
        }
    }

    public struct FeedbackData
    {
        public string Text;
        public Color Color;
        public float Duration;
        public Vector2 Position;
    }
}

