using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Game
{
    public class GameHeroSelectionArrow : MonoBehaviour
    {
        [SerializeField] private float _yPositionMovementAmount = 0.3f;
        [SerializeField] private float _animDuration = 0.4f;
        private Vector3 _startPosition;

        public void EnableArrow()
        {
            EnableArrow(true);
            StartArrowAnimation();
        }

        public void DisableArrow()
        {
            if (!gameObject.activeSelf) return;

            KillArrowAnimation();
            EnableArrow(false);
        }

        public void EnableArrow(bool enable)
        {
            gameObject.SetActive(enable);
        }

        private void StartArrowAnimation()
        {
            _startPosition = transform.position;
            transform.DOMoveY(_startPosition.y - _yPositionMovementAmount, _animDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuart);
        }

        private void KillArrowAnimation()
        {
            transform.DOKill();
            transform.position = _startPosition;
        }
    }
}
