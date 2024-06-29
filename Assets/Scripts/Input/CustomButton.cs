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

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Pointer Down");
            _hasExitedBorders = false;
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Pointer Exit");
            _hasExitedBorders = true;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Pointer Up");
            if (_hasExitedBorders) { return; }
            OnClick?.Invoke();
        }
    }

}
