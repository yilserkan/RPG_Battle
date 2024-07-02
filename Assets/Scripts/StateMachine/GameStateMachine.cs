using DG.Tweening;
using RPGGame.Game;
using RPGGame.Hero;
using RPGGame.Level;
using RPGGame.Player;
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
        private GameStates _currentStateType = GameStates.None;

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

        private void Awake()
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

        private void Update()
        {
            _currentState?.OnUpdate();
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
            _currentStateType = newState;
            _currentState?.OnEnter();
        }

        public void DelayedSwitchState(GameStates newState, float delay)
        {
            DOVirtual.DelayedCall(delay, () => SwitchState(newState));
        }

        public void SetGameHeroesDict(Dictionary<HeroTeam, GameHero[]> heroes)
        {
            _gameHeroesDict = heroes;
        }

        public GameHero[] GetAliveHeroesOfTeam(HeroTeam team)
        {
            var heroes = GameHeroesDic[team];
            return heroes.Where(hero => !hero.HealthController.IsDead).ToArray();
        }

        public GameHero GetRandomGameHero(HeroTeam team)
        {
            var aliveHeroes = GetAliveHeroesOfTeam(team);
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
            _currentStateType = GameStates.None;
        }

        public void SaveGame()
        {
            var playerHeroeDatas = CreateGameHeroDataForTeam(HeroTeam.Player);
            var enemyHeroeDatas = CreateGameHeroDataForTeam(HeroTeam.Enemy);
            var levelData = new LevelData(playerHeroeDatas, enemyHeroeDatas, _currentStateType);
            PlayerData.SetActiveLevelData(levelData);
        }

        private GameHeroData[] CreateGameHeroDataForTeam(HeroTeam team)
        {
            var heroes = _gameHeroesDict[team];
            var playerHeroes = new GameHeroData[heroes.Length];

            for (int i = 0; i < playerHeroes.Length; i++)
            {
                playerHeroes[i] = new GameHeroData(heroes[i]);
            }

            return playerHeroes;
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

