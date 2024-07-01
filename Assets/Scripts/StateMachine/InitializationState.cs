using RPGGame.Game;

namespace RPGGame.StateMachine
{
    public class InitializationState : BaseState
    {
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

            _stateMachine.HeroSpawner.CreateHeroes(levelData.PlayerHeroes);
            _stateMachine.HeroSpawner.CreateHeroes(levelData.EnemyHeroes);

            var nextState = GetNextState(levelData);
            _stateMachine.SwitchState(nextState);
        }

        private GameStates GetNextState(LevelData levelData)
        {
            if(levelData.CurrentState == (int)GameStates.None)
            {
                return GameStates.None;
            }

            return (GameStates)levelData.CurrentState;
        }
    }
}
