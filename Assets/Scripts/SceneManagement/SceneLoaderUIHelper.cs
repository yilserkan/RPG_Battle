using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.SceneManagement
{
    public class SceneLoaderUIHelper : MonoBehaviour, IProgress<float>
    {
        [SerializeField] private Image _loadingBarFillImage;

        private void Awake()
        {
            EnableLoadingPanel(false);
        }

        public void EnableLoadingPanel(bool enabled)
        {
            gameObject.SetActive(enabled);
        }

        public void Report(float value)
        {
            UpdateLoadingBar(value);
        }

        private void UpdateLoadingBar(float value)
        {
            _loadingBarFillImage.fillAmount = value;
        }
    }
}