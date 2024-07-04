using DG.Tweening;
using RPGGame.Pool;
using RPGGame.Utils;
using System.Collections;
using TMPro;
using UnityEngine;

namespace RPGGame.Feedback
{
    public class Feedback : Poolable
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _feedbackText;

        private FeedbackData _data;

        public void Initialize(FeedbackData data)
        {
            _data = data;
            _feedbackText.text = data.Text;
            _feedbackText.color = data.Color;
            _rectTransform.position = data.Position;
            StartAnimation();
            StartCoroutine(StartLifeTime());
        }

        private void StartAnimation()
        {
            var halfOfDuration = _data.Duration / 2f;
            transform.localScale = Vector3.zero;
            transform.DOScale(1, halfOfDuration).SetEase(Ease.OutElastic);
        }

        private IEnumerator StartLifeTime()
        {
            yield return CoroutineHelper.GetWaitForSeconds(_data.Duration);
            Release();
        }

        private void Release()
        {
            ObjectPool.ReturnToPool(this);
        }
    }
}
