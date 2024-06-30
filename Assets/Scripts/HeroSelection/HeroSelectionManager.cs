using RPGGame.Player;
using RPGGame.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.HeroSelection
{
    public class HeroSelectionManager : MonoBehaviour, IObserver<bool>
    {
        [SerializeField] private HeroSelectionSlot[] _heroSlots;
        [SerializeField] private CustomButton _playButton;

        private List<HeroSelectionSlot> _selectedSlots;
        private Observable<bool> _hasSelectedAllHeroes;
        private const int MAX_SELECTABLE_HEROES_COUNT = 3;

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
            _selectedSlots = new List<HeroSelectionSlot>();
        }

        private void InitializeSlots()
        {
            var heroes = PlayerData.GetPlayerHeroes();

            for (int i = 0; i < _heroSlots.Length; i++)
            {
                var hero = heroes.Length > i ? heroes[i] : null;
                _heroSlots[i].SetupSlot(hero);
                _hasSelectedAllHeroes.AddListener(_heroSlots[i]);
            }
        }

        private void HandleOnSlotSelected(HeroSelectionSlot slot)
        {
            _selectedSlots.Add(slot);

            _hasSelectedAllHeroes.Value = _selectedSlots.Count == MAX_SELECTABLE_HEROES_COUNT;
        }

        private void HandleOnSlotUnselected(HeroSelectionSlot slot)
        {
            _selectedSlots.Remove(slot);

            _hasSelectedAllHeroes.Value = _selectedSlots.Count == MAX_SELECTABLE_HEROES_COUNT;
        }

        public void Notify(bool hasSelectedAllHeroes)
        {
            _playButton.Interactable = hasSelectedAllHeroes;
        }

        private void AddListeners()
        {
            HeroSelectionSlot.OnSlotSelectedEvent += HandleOnSlotSelected;
            HeroSelectionSlot.OnSlotUnselectedEvent += HandleOnSlotUnselected;
        }

        private void RemoveListeners()
        {
            HeroSelectionSlot.OnSlotSelectedEvent -= HandleOnSlotSelected;
            HeroSelectionSlot.OnSlotUnselectedEvent -= HandleOnSlotUnselected;
        }

    }
}

