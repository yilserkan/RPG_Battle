using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionSlot : MonoBehaviour
    {
        [SerializeField] private HeroSelectionSlotUI _slotUI;
        [SerializeField] private Image _heroIcon;
        [SerializeField] private TextMeshProUGUI _heroNameText;
        [SerializeField] private CustomHoldableButton _customHoldableButton;

        private Hero.Hero _hero;
        public static event Action<Hero.Hero> RequestHeroPopupEvent;

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
            var isSlotOccupied = hero != null;

            if (isSlotOccupied)
            {
                SetupOccupiedSlot(hero);
            }
            else
            {
                SetupEmptySlot();
            }
        }

        private void SetupOccupiedSlot(Hero.Hero hero)
        {
            _heroIcon.gameObject.SetActive(true);
            _heroIcon.sprite = hero.Settings.HeroSprite;
            _heroNameText.text = hero.Settings.Name;
        }

        private void SetupEmptySlot()
        {
            _heroIcon.gameObject.SetActive(false);
        }

        private void RequestHeroPopup()
        {
            RequestHeroPopupEvent?.Invoke(_hero);
        }

        private void AddListeners()
        {
            _customHoldableButton.OnHold += RequestHeroPopup;
        }

        private void RemoveListeners()
        {
            _customHoldableButton.OnHold -= RequestHeroPopup;
        }
    }
}
