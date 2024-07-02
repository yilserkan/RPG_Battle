using RPGGame.Player;
using RPGGame.ResultScreen;
using RPGGame.StateMachine;
using RPGGame.Utils;
using RPGGame.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionManager : MonoBehaviour, Observer.IObserver<bool>
    {
        [SerializeField] private HeroSelectionSlot[] _heroSlots;
        [SerializeField] private CustomButton _playButton;
        [SerializeField] private GameObject _parent; 

        private List<Hero.Hero> _selectedHeroes;
        private Observable<bool> _hasSelectedAllHeroes;
        private const int MAX_SELECTABLE_HEROES_COUNT = 3;

        public static event Action<List<Hero.Hero>> OnStartGameEvent;

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
            _hasSelectedAllHeroes = new Observable<bool>(false, this);
            InitializeSlots();
            _selectedHeroes = new List<Hero.Hero>();
        }

        private void InitializeSlots()
        {
            var heroes = PlayerData.GetPlayerHeroes();

            for (int i = 0; i < _heroSlots.Length; i++)
            {
                var hero = heroes.Count > i ? heroes[i] : null;
                _heroSlots[i].SetupSlot(hero);
                _hasSelectedAllHeroes.AddListener(_heroSlots[i]);
            }

            EnableHeroSelectionPanel(true);
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
        }


        private void HandleOnGameInitialized()
        {
            EnableHeroSelectionPanel(false);
        }

        public void Notify(bool hasSelectedAllHeroes)
        {
            _playButton.Interactable = hasSelectedAllHeroes;
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

