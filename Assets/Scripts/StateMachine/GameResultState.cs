using RPGGame.Game;
using RPGGame.Player;
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
            bool hasPlayerWon = _winnerTeam == GetWinnerTeam();

            if(hasPlayerWon)
            {
                PlayerWon();
            }

            PlayerData.UpdateGameData();
            ShowResultScreen();
        }

        private void PlayerWon()
        {
            var heroes = _stateMachine.GetAliveHeroesOfTeam(HeroTeam.Player);
            PlayerData.LocallyIncreaseHeroExperience(heroes);
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
