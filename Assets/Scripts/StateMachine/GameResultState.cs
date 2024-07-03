using RPGGame.Game;
using RPGGame.Player;
using System;

namespace RPGGame.StateMachine
{
    public class GameResultState : BaseState
    {
        public static event Action<HeroTeam> OnShowResultScreen;

        public GameResultState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            ShowResultScreen();
        }

  
        private void ShowResultScreen()
        {
            OnShowResultScreen?.Invoke(GetWinnerTeam());
        }

        private HeroTeam GetWinnerTeam()
        {
            var playerLost = _stateMachine.CheckIfAllHeroesDiedInTeam(HeroTeam.Player);
            return playerLost ? HeroTeam.Enemy : HeroTeam.Player;
        }
    }
}
