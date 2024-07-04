using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionSlot : MonoBehaviour, Observer.IObserver<bool>
    {
        [SerializeField] private HeroSelectionSlotUI _slotUI;
     
        [SerializeField] private CustomHoldableButton _slotButton;

        private Hero.Hero _hero;
        private bool _isSelected;
        private bool _canSelect;

        public static event Action<Hero.Hero> RequestHeroPopupEvent;
        public static event Action<Hero.Hero> OnSlotSelectedEvent;
        public static event Action<Hero.Hero> OnSlotUnselectedEvent;


        private void OnEnable()
        {
            AddListeners();   
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        public void SetupSlot(Hero.Hero hero)
        {
            _hero = hero;
            _slotUI.SetupUI(hero);
            _canSelect = true;
            _isSelected = false;
            ToggleSlotButtonInteractable(true);
        }

        public void ResetSlot()
        {
            _hero = null;
            _slotUI.ResetUI();
            _isSelected = false;
            _canSelect = true;
        }

        public void Notify(bool hasSelectedAllHeroes)
        {
            _canSelect = !hasSelectedAllHeroes;

            if(hasSelectedAllHeroes && !_isSelected)
            {
                _slotUI.ShowUnselectableUI();
                ToggleSlotButtonInteractable(false);
            }
            else if (!hasSelectedAllHeroes && _hero != null)
            {
                _slotUI.ShowSelectableUI();
                ToggleSlotButtonInteractable(true);
            }
        }

        private void HandleOnSlotClicked()
        {
            if (_isSelected)
            {
                UnselectSlot();
            }
            else
            {
                SelectSlot();
            }
        }

        private void SelectSlot()
        {
            if (!_canSelect){return;}

            _isSelected = true;
            _slotUI.ShowSelectedUI();
            OnSlotSelectedEvent?.Invoke(_hero);
        }

        private void UnselectSlot()
        {
            _isSelected = false;
            _slotUI.ShowUnselectedUI();
            OnSlotUnselectedEvent?.Invoke(_hero);
        }

        private void RequestHeroPopup()
        {
            RequestHeroPopupEvent?.Invoke(_hero);
        }

        private void ToggleSlotButtonInteractable(bool toggle)
        {
            _slotButton.Interactable = toggle && _hero != null;
        }

        private void AddListeners()
        {
            _slotButton.OnClick += HandleOnSlotClicked;
            _slotButton.OnHold += RequestHeroPopup;
        }

        private void RemoveListeners()
        {
            _slotButton.OnClick -= HandleOnSlotClicked;
            _slotButton.OnHold -= RequestHeroPopup;
        }
    }
}
