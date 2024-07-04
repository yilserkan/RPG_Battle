using RPGGame.Feedback;
using RPGGame.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RPGGame.Feedback
{
    public class LevelUpFeedbackController
    {
        private Queue<LevelUpData> _levelUpDataQueue = new Queue<LevelUpData> ();
        private const int LEVEL_UP_DURATION = 3;

        public static event Action<FeedbackData> RequestLevelUpFeedback;

        private void HandleOnPlayerLeveldUp(LevelUpData data)
        {
            _levelUpDataQueue.Enqueue(data);
        }

        public async Task TryShowLevelUpFeedback()
        {
            while(_levelUpDataQueue.Count > 0)
            {
                var data = _levelUpDataQueue.Dequeue();
                var feedbackData = CreateFeedbackData(data);
                RequestLevelUpFeedback?.Invoke(feedbackData);
                await Task.Delay(LEVEL_UP_DURATION * 1000);
            }
        }

        private FeedbackData CreateFeedbackData(LevelUpData data)
        {
            var levelUpText = $"{data.HeroName} Leveled Up \n";
            for (int i = 0; i < data.ModifiedAttributes.Length; i++)
            {
                var attr = data.ModifiedAttributes[i];
                var sign = attr.IncreaseAmount > 0 ? "+" : "";
                levelUpText += $"{attr.Type} : {sign}{attr.IncreaseAmount:f2} \n";
            }

            FeedbackData feedbackData = new FeedbackData()
            {
                Color = UnityEngine.Color.white,
                Duration = LEVEL_UP_DURATION,
                Position = UnityEngine.Vector2.zero,
                PositionType = FeedbackPositionType.AnchoredPostiion,
                Text = levelUpText
            };

            return feedbackData;
        }

        public void AddListeners()
        {
            GameHeroLevelController.OnPlayerLeveldUp += HandleOnPlayerLeveldUp;
        }

        public void RemoveListeners()
        {
            GameHeroLevelController.OnPlayerLeveldUp -= HandleOnPlayerLeveldUp;
        }
    }
}
