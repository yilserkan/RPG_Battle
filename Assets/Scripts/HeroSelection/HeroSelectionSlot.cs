using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionSlot : MonoBehaviour
    {
        [SerializeField] private Image _heroIcon;
        [SerializeField] private TextMeshProUGUI _heroNameText;

        public void SetupSlot(Hero.Hero hero)
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
            _heroNameText.text = hero._settings.Name;
        }

        private void SetupEmptySlot()
        {
            _heroIcon.gameObject.SetActive(false);
        }
    }
}
