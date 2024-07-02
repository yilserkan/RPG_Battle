using DG.Tweening;
using RPGGame.Game;
using RPGGame.Level;
using RPGGame.Pool;
using RPGGame.ResultScreen;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace RPGGame.StateMachine
{
    public class GameStateMachine : MonoBehaviour
    {
        [SerializeField] private GameHeroSpawner _heroSpawner;

        public GameHeroSpawner HeroSpawner => _heroSpawner;

        private BaseState _currentState;

        private Dictionary<GameStates, BaseState> _states;

        private LevelData _levelData;
        public LevelData LevelData => _levelData;
        private Dictionary<HeroTeam, GameHero[]> _gameHeroesDict;
        public Dictionary<HeroTeam, GameHero[]> GameHeroesDic => _gameHeroesDict;

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
            _states =
            new Dictionary<GameStates, BaseState>()
            {
                { GameStates.Initial, new InitializationState(this) },
                { GameStates.PlayerTurn, new PlayerTurnState(this) },
                { GameStates.EnemyTurn, new EnemyTurnState(this) },
                { GameStates.GameResult, new GameResultState(this) }
            };

            _gameHeroesDict = new Dictionary<HeroTeam, GameHero[]>();
        }

        public void StartLevel(LevelData levelData)
        {
            _levelData = levelData;
            SwitchState(GameStates.Initial);
        }

        public void SwitchState(GameStates newState)
        {
            _currentState?.OnExit();
            _currentState = _states[newState];
            _currentState?.OnEnter();
        }

        public void DelayedSwitchState(GameStates newState, float delay)
        {
            DOVirtual.DelayedCall(delay, () => SwitchState(newState));
        }

        private void OnStateEnter()
        {
            _currentState?.OnEnter();
        }

        private void OnStateExit()
        {
            _currentState?.OnExit();
        }

        private void OnStateUpdate()
        {
            _currentState?.OnUpdate();
        }

        public void SetGameHeroesDict(Dictionary<HeroTeam, GameHero[]> heroes)
        {
            _gameHeroesDict = heroes;
        }

        public GameHero GetRandomGameHero(HeroTeam team)
        {
            var heroes = GameHeroesDic[team];
            var aliveHeroes = heroes.Where(hero => !hero.HealthController.IsDead).ToArray();
            int randIndex = Random.Range(0, aliveHeroes.Length);
            return aliveHeroes[randIndex];
        }

        public bool CheckIfAllHeroesDiedInTeam(HeroTeam team)
        {
            var heroes = GameHeroesDic[team];
            var aliveHeroes = heroes.Where(hero => !hero.HealthController.IsDead).ToArray();
            return aliveHeroes.Length == 0;
        }

        private void ResetGame()
        {
            foreach (var gameHeroes in _gameHeroesDict)
            {
                for (int i = 0; i < gameHeroes.Value.Length; i++)
                {
                    var hero = gameHeroes.Value[i];
                    hero.ResetHero();
                }
            }

            _gameHeroesDict.Clear();
            _levelData = null;
            _currentState = null;
        }

        private void AddListeners()
        {
            ResultScreenUIManager.OnReturnToHeroSelectionScreen += ResetGame;
        }

        private void RemoveListeners()
        {
            ResultScreenUIManager.OnReturnToHeroSelectionScreen -= ResetGame;
        }
    }

    public enum GameStates
    {
        None,
        Initial,
        PlayerTurn,
        EnemyTurn,
        GameResult
    }
}

