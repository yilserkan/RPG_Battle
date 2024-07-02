using RPGGame.Game;
using System;
using System.Collections.Generic;

namespace RPGGame.StateMachine
{
    public class InitializationState : BaseState
    {
        public static event Action OnGameInitialized;

        public InitializationState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            InitializeGameState();
        }

        private void InitializeGameState()
        {
            var levelData = _stateMachine.LevelData;

            var playerHeroes = _stateMachine.HeroSpawner.CreateHeroes(levelData.PlayerHeroes);
            var enemyHeroes = _stateMachine.HeroSpawner.CreateHeroes(levelData.EnemyHeroes);

            var heroesDict = new Dictionary<HeroTeam, GameHero[]>();
            heroesDict.Add(HeroTeam.Player, playerHeroes);
            heroesDict.Add(HeroTeam.Enemy, enemyHeroes);

            _stateMachine.SetGameHeroesDict(heroesDict);

            var nextState = GetNextState(levelData);
            OnGameInitialized?.Invoke();
            _stateMachine.SwitchState(nextState);
        }

        private GameStates GetNextState(LevelData levelData)
        {
            if(levelData.CurrentState == (int)GameStates.None)
            {
                return GameStates.PlayerTurn;
            }

            return (GameStates)levelData.CurrentState;
        }
    }
}
