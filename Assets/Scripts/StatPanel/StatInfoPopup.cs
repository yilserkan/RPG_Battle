using RPGGame.Game;
using RPGGame.HeroSelection;
using RPGGame.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.Stats
{
    public class StatInfoPopup : MonoBehaviour
    {
        [SerializeField] private GameObject _parent;
        [SerializeField] private StatInfo[] statInfos;

        [SerializeField] private TextMeshProUGUI _heroNameText;
        [SerializeField] private TextMeshProUGUI _heroLevelText;
        [SerializeField] private TextMeshProUGUI _heroExpText;
        [SerializeField] private Image _heroSprite;

        [SerializeField] private CustomButton _closeButton;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();  
        }

        public void HandleOnInfoPopupRequested(Hero.Hero hero)
        {
            SetPlayerDetails(hero);
            for (int i = 0; i < statInfos.Length; i++)
            {
                var statType = statInfos[i].Type;
                var attrValue = hero.Stats.CalculateAttributeValue(statType);
                statInfos[i].Setup(attrValue);
            }

            _parent.SetActive(true);
        }

        private void HandleOnCloseButtonClicked()
        {
            _parent.SetActive(false);
        }

        private void SetPlayerDetails(Hero.Hero hero)
        {
            _heroNameText.text = $"{hero.Settings.Name}";
            _heroLevelText.text = $"{hero.Level}";
            _heroExpText.text = $"{hero.Experience}";
            _heroSprite.sprite = hero.Settings.HeroSprite;
        }


        private void AddListeners()
        {
            HeroSelectionSlot.RequestHeroPopupEvent += HandleOnInfoPopupRequested;
            GameHeroSelectionController.RequestHeroPopupEvent += HandleOnInfoPopupRequested;
            _closeButton.OnClick += HandleOnCloseButtonClicked;
        }

        private void RemoveListeners()
        {
            HeroSelectionSlot.RequestHeroPopupEvent -= HandleOnInfoPopupRequested;
            GameHeroSelectionController.RequestHeroPopupEvent -= HandleOnInfoPopupRequested;
            _closeButton.OnClick -= HandleOnCloseButtonClicked;
        }
    }
}

