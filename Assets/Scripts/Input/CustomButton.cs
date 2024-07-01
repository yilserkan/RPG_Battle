using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGGame.Utils
{
    public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        public event Action OnClick;

        protected bool _hasExitedBorders;
        public bool Interactable = true;

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable) { return; }
            PlayPointerDownAnimation();
            Debug.Log("Pointer Down");
            _hasExitedBorders = false;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Pointer Exit");
            _hasExitedBorders = true;
            PlayPointerUpAnimation();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Pointer Up");
            if (_hasExitedBorders || !Interactable) { return; }
            PlayPointerUpAnimation();
            OnClick?.Invoke();
        }

        private void PlayPointerDownAnimation()
        {
            transform.DOScale(.9f, .1f);
        }

        private void PlayPointerUpAnimation()
        {
            transform.DOScale(1, .1f);
        }
    }
}
