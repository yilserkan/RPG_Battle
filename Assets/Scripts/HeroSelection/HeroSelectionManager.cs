using RPGGame.Player;
using RPGGame.ResultScreen;
using RPGGame.StateMachine;
using RPGGame.Utils;
using RPGGame.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Feedback;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionManager : MonoBehaviour
    {
        [SerializeField] private HeroSelectionSlot[] _heroSlots;
        [SerializeField] private CustomButton _playButton;
        [SerializeField] private GameObject _parent; 

        private List<Hero.Hero> _selectedHeroes;
        private Observable<bool> _hasSelectedAllHeroes;
        private const int MAX_SELECTABLE_HEROES_COUNT = 3;

        public static event Action<List<Hero.Hero>> OnStartGameEvent;
        public static event Action<FeedbackData> RequestSelectHeroesFeedbackEvent;

        private void OnEnable()
        {
            AddListeners();   
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        private void Start()
        {
            _playButton.Interactable = true;
            _hasSelectedAllHeroes = new Observable<bool>(false);
            InitializeSlots();
            _selectedHeroes = new List<Hero.Hero>();

            if(!PlayerData.HasActiveLevelData())
                EnableHeroSelectionPanel(true);
        }

        private async void InitializeSlots()
        {
            await PlayerData.SetPlayerHeroes();
            var heroes = PlayerData.GetPlayerHeroes();

            for (int i = 0; i < _heroSlots.Length; i++)
            {
                var hero = heroes.Count > i ? heroes[i] : null;
                _heroSlots[i].SetupSlot(hero);
                _hasSelectedAllHeroes.AddListener(_heroSlots[i]);
            }
        }

        private void HandleOnSlotSelected(Hero.Hero hero)
        {
            _selectedHeroes.Add(hero);

            _hasSelectedAllHeroes.Value = _selectedHeroes.Count == MAX_SELECTABLE_HEROES_COUNT;
        }

        private void HandleOnSlotUnselected(Hero.Hero hero)
        {
            _selectedHeroes.Remove(hero);

            _hasSelectedAllHeroes.Value = _selectedHeroes.Count == MAX_SELECTABLE_HEROES_COUNT;
        }

        private void HandleOnPlayButtonClicked()
        {
            if(_selectedHeroes.Count < 3)
            {
                RequestSelectHeroesFeedback();
                return;
            }

            OnStartGameEvent?.Invoke(_selectedHeroes);
            ResetSelections();
        }

        private void ResetSelections()
        {
            _selectedHeroes.Clear();

            for (int i = 0; i < _heroSlots.Length; i++)
            {
                _heroSlots[i].ResetSlot();
                _hasSelectedAllHeroes.RemoveListener(_heroSlots[i]);
            }
            _hasSelectedAllHeroes.Value = false;
        }

        private void EnableHeroSelectionPanel(bool enabled)
        {
            _parent.SetActive(enabled);
        }

        private void HandleOnReturnToSelectionScreen()
        {
            InitializeSlots();
            EnableHeroSelectionPanel(true);
        }


        private void HandleOnGameInitialized()
        {
            EnableHeroSelectionPanel(false);
        }

        private void RequestSelectHeroesFeedback()
        {
            var feedbackData = new FeedbackData()
            {
                Text = "SELECT 3 HEROES TO START",
                Color = Color.white,
                Position = Vector2.zero,
                PositionType = FeedbackPositionType.AnchoredPostiion,
                Duration = 1
            };
            RequestSelectHeroesFeedbackEvent?.Invoke(feedbackData);
        }

        private void AddListeners()
        {
            HeroSelectionSlot.OnSlotSelectedEvent += HandleOnSlotSelected;
            HeroSelectionSlot.OnSlotUnselectedEvent += HandleOnSlotUnselected;
            ResultScreenUIManager.OnReturnToHeroSelectionScreen += HandleOnReturnToSelectionScreen;
            InitializationState.OnGameInitialized += HandleOnGameInitialized;
            _playButton.OnClick += HandleOnPlayButtonClicked;
        }

        private void RemoveListeners()
        {
            HeroSelectionSlot.OnSlotSelectedEvent -= HandleOnSlotSelected;
            HeroSelectionSlot.OnSlotUnselectedEvent -= HandleOnSlotUnselected;
            ResultScreenUIManager.OnReturnToHeroSelectionScreen -= HandleOnReturnToSelectionScreen;
            InitializationState.OnGameInitialized -= HandleOnGameInitialized;
            _playButton.OnClick -= HandleOnPlayButtonClicked;
        }
    }
}

