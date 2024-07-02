using RPGGame.Game;
using RPGGame.StateMachine;
using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPGGame.ResultScreen
{
    public class ResultScreenUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _resultScreenText;
        [SerializeField] private CustomButton _continueButton;
        [SerializeField] private GameObject _parent;

        private const string VICTORY_TEXT = "VICTORY";
        private const string LOST_TEXT = "LOST";

        public static event Action OnReturnToHeroSelectionScreen;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();  
        }

        private void HandleOnShowResultScreen(HeroTeam winnerTeam)
        { 
            var resultText = winnerTeam == HeroTeam.Player ? VICTORY_TEXT : LOST_TEXT;
            _resultScreenText.text = resultText;
            _parent.SetActive(true);
        }

        private void HandleOnContinueButtonClicked()
        {
            OnReturnToHeroSelectionScreen?.Invoke();
            _parent.SetActive(false);
        }

        private void AddListeners()
        {
            _continueButton.OnClick += HandleOnContinueButtonClicked;
            GameResultState.OnShowResultScreen += HandleOnShowResultScreen;
        }

        private void RemoveListeners()
        {
            _continueButton.OnClick -= HandleOnContinueButtonClicked;
            GameResultState.OnShowResultScreen -= HandleOnShowResultScreen;
        }
    }
}
