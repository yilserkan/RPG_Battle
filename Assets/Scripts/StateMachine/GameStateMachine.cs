using RPGGame.Game;
using RPGGame.Level;
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

        private void Start()
        {
            _states =
            new Dictionary<GameStates, BaseState>()
            {
                { GameStates.Initial, new InitializationState(this) },
                { GameStates.PlayerTurn, new PlayerTurnState(this) },
                { GameStates.EnemyTurn, new EnemyTurnState(this) }
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
            var enemyHeroes = GameHeroesDic[team];
            var heroes = enemyHeroes.Where(hero => hero.Vitality > 0).ToArray();
            int randIndex = Random.Range(0, heroes.Length);
            return heroes[randIndex];
        }

    }

    public enum GameStates
    {
        None,
        Initial,
        PlayerTurn,
        EnemyTurn
    }
}

