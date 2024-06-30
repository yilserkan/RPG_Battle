using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionSlotUI : MonoBehaviour
    {
        [SerializeField] private HeroSelectionSlotUISettings _uiSettings;
        [SerializeField] private Image _selectedSlotFrameImg;
        [SerializeField] private Image _selectedSlotBgImg;
        [SerializeField] private Image _heroIcon;
        [SerializeField] private TextMeshProUGUI _heroNameText;

        public void SetupUI(Hero.Hero hero)
        {
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

        public void ShowSelectedUI()
        {
            _selectedSlotFrameImg.color = _uiSettings.FrameSelectedColor;
        }

        public void ShowUnselectedUI()
        {
            _selectedSlotFrameImg.color = _uiSettings.FrameUnselectedColor;
        }

        public void ShowUnselectableUI()
        {
            _selectedSlotBgImg.color = _uiSettings.SlotUnselectableColor;
        }

        public void ShowSelectableUI()
        {
            _selectedSlotBgImg.color = _uiSettings.SlotDefaultColor;
        }
    }
}