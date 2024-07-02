using RPGGame.Game;
using System;

namespace RPGGame.StateMachine
{
    public class GameResultState : BaseState
    {
        private HeroTeam _winnerTeam;

        public static event Action<HeroTeam> OnShowResultScreen;

        public GameResultState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            _winnerTeam = GetWinnerTeam();

            if(_winnerTeam == HeroTeam.Player)
            {
                PlayerWon();
            }

            ShowResultScreen();
        }

        private void PlayerWon()
        {

        }
  
        private void ShowResultScreen()
        {
            OnShowResultScreen?.Invoke(_winnerTeam);
        }

        private HeroTeam GetWinnerTeam()
        {
            var playerLost = _stateMachine.CheckIfAllHeroesDiedInTeam(HeroTeam.Player);
            return playerLost ? HeroTeam.Enemy : HeroTeam.Player;
        }
    }
}
