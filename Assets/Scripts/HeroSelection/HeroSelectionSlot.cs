using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionSlot : MonoBehaviour, Utils.IObserver<bool>
    {
        [SerializeField] private HeroSelectionSlotUI _slotUI;
     
        [SerializeField] private CustomHoldableButton _customHoldableButton;

        private Hero.Hero _hero;
        private bool _isSelected;
        private bool _canSelect;

        public static event Action<Hero.Hero> RequestHeroPopupEvent;
        public static event Action<HeroSelectionSlot> OnSlotSelectedEvent;
        public static event Action<HeroSelectionSlot> OnSlotUnselectedEvent;


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
            ToggleSlotButtonInteractable(true);
        }

        public void Notify(bool hasSelectedAllHeroes)
        {
            _canSelect = !hasSelectedAllHeroes;

            if(hasSelectedAllHeroes && !_isSelected)
            {
                _slotUI.ShowUnselectableUI();
                ToggleSlotButtonInteractable(false);
            }
            else if (!hasSelectedAllHeroes)
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
            OnSlotSelectedEvent?.Invoke(this);
        }

        private void UnselectSlot()
        {
            _isSelected = false;
            _slotUI.ShowUnselectedUI();
            OnSlotUnselectedEvent?.Invoke(this);
        }

        private void RequestHeroPopup()
        {
            RequestHeroPopupEvent?.Invoke(_hero);
        }

        private void ToggleSlotButtonInteractable(bool toggle)
        {
            _customHoldableButton.Interactable = toggle && _hero != null;
        }

        private void AddListeners()
        {
            _customHoldableButton.OnClick += HandleOnSlotClicked;
            _customHoldableButton.OnHold += RequestHeroPopup;
        }

        private void RemoveListeners()
        {
            _customHoldableButton.OnClick -= HandleOnSlotClicked;
            _customHoldableButton.OnHold -= RequestHeroPopup;
        }
    }
}
