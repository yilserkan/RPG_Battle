using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Game
{
    public class GameHeroSelectionArrow : MonoBehaviour
    {
        public void EnableArrow()
        {
            EnableArrow(true);
            StartArrowAnimation();
        }

        public void DisableArrow()
        {
            EnableArrow(false);
            KillArrowAnimation();
        }

        public void EnableArrow(bool enable)
        {
            gameObject.SetActive(enable);
        }

        private void StartArrowAnimation()
        {
            transform.DOMoveY(transform.position.y -.3f, .4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutQuart);
        }

        private void KillArrowAnimation()
        {
            transform.DOKill();
        }
    }
}
