using RPGGame.StateMachine;
using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Game
{
    public class GameHeroSelectionController : MonoBehaviour
    {
        [SerializeField] private GameHero _gameHero;
        [SerializeField] private CustomHoldableButton _heroButton;
        [SerializeField] private GameHeroSelectionArrow _selectionArrow;

        public static event Action<Hero.Hero> RequestHeroPopupEvent;
        public static event Action<GameHero> OnHeroSelectedToAttack;
        private bool _isSelectable;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void Awake()
        {
            _selectionArrow.DisableArrow();
        }

        private void HandleOnEnablePlayerSelection()
        {
            SetIsHeroSelectable(true);

            if(_gameHero.Team == HeroTeam.Player && !_gameHero.HealthController.IsDead)
                _selectionArrow.EnableArrow();
        }
        private void HandleOnDisablePlayerSelection()
        {
            SetIsHeroSelectable(false);

            if (_gameHero.Team == HeroTeam.Player && !_gameHero.HealthController.IsDead)
                _selectionArrow.DisableArrow();
        }

        private void SetIsHeroSelectable(bool value)
        {
            _isSelectable = value && _gameHero.Team == HeroTeam.Player;
        }

        private void HandleOnPlayerClicked()
        {
            if (!_isSelectable) return;

            OnHeroSelectedToAttack?.Invoke(_gameHero);
        }

        private void HandleOnPlayerHold()
        {
            RequestHeroPopupEvent?.Invoke(_gameHero.Hero);
        }

        private void AddListeners()
        {
            PlayerTurnState.OnEnablePlayerSelectionEvent += HandleOnEnablePlayerSelection;
            PlayerTurnState.OnDisablePlayerSelectionEvent += HandleOnDisablePlayerSelection;
            _heroButton.OnClick += HandleOnPlayerClicked;
            _heroButton.OnHold += HandleOnPlayerHold;
        }

        private void RemoveListeners()
        {
            PlayerTurnState.OnEnablePlayerSelectionEvent -= HandleOnEnablePlayerSelection;
            PlayerTurnState.OnDisablePlayerSelectionEvent -= HandleOnDisablePlayerSelection;
            _heroButton.OnClick -= HandleOnPlayerClicked;
            _heroButton.OnHold -= HandleOnPlayerHold;
        }
    }
}
