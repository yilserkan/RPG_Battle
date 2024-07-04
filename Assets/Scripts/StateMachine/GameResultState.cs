using RPGGame.Feedback;
using RPGGame.Game;
using RPGGame.Player;
using System;

namespace RPGGame.StateMachine
{
    public class GameResultState : BaseState
    {
        public static event Action<HeroTeam> OnShowResultScreen;

        private LevelUpFeedbackController _levelUpFeedbackController = new LevelUpFeedbackController();
        public GameResultState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override async void OnEnter()
        {
            IncreaseHeroExperiences();
            await _levelUpFeedbackController.TryShowLevelUpFeedback();
            ShowResultScreen();
            PlayerData.LocallyIncreasePlayedMatchCounts();
        }

        private void IncreaseHeroExperiences()
        {
            _levelUpFeedbackController.AddListeners();

            var playerHeroes = _stateMachine.GetAliveHeroesOfTeam(HeroTeam.Player);
            for (int i = 0; i < playerHeroes.Length; i++)
            {
                playerHeroes[i].LevelController.LocallyIncreaseExperience();
            }

            _levelUpFeedbackController.RemoveListeners();
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
