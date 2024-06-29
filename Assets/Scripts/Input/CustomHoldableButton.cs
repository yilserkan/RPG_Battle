using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPGGame.Utils
{
    public class CustomHoldableButton : CustomButton
    {
        public event Action OnHold;
        private Coroutine _holdCoroutine;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            StartHoldTimer();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            StopHoldTimer();
            base.OnPointerUp(eventData);
        }

        private void StartHoldTimer()
        {
            _holdCoroutine = StartCoroutine(HoldTimer());
        }

        private void StopHoldTimer()
        {
            if (_holdCoroutine == null) { return; }
            StopCoroutine(_holdCoroutine);
        }

        private IEnumerator HoldTimer()
        {
            float passedTime = 0;
            while (passedTime < 2)
            {
                yield return new WaitForEndOfFrame();
                passedTime += Time.deltaTime;
            }

            _hasExitedBorders = true;
            OnHold?.Invoke();
            _holdCoroutine = null;
            Debug.Log("Hold For 3 Seconds");
        }
    }
}
