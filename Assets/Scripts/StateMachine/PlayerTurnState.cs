using RPGGame.Game;
using System;

namespace RPGGame.StateMachine
{
    public class PlayerTurnState : BaseState
    {
        public static event Action OnEnablePlayerSelectionEvent;
        public static event Action OnDisablePlayerSelectionEvent;

        private const float _switchStateDelay = .5f;

        public PlayerTurnState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _stateMachine.SaveGame();
            AddListeners();
            OnEnablePlayerSelectionEvent?.Invoke();
        }
        public override void OnExit()
        {
            base.OnExit();
            RemoveListeners();
        }
        private void HandleOnHeroSelected(GameHero hero)
        {
            OnDisablePlayerSelectionEvent?.Invoke();

            var target = _stateMachine.GetRandomGameHero(HeroTeam.Enemy);
            hero.SkillController.Attack(target);
        }

        private void OnHeroCompletedAttack()
        {
            _stateMachine.DelayedSwitchState(GetNextState(), _switchStateDelay);
        }

        private GameStates GetNextState()
        {
            bool allEnemiesDied = _stateMachine.CheckIfAllHeroesDiedInTeam(HeroTeam.Enemy);
            return allEnemiesDied ? GameStates.GameResult : GameStates.EnemyTurn;
        }

        private void AddListeners()
        {
            GameHeroSelectionController.OnHeroSelectedToAttack += HandleOnHeroSelected;
            GameHeroSkillController.OnHeroCompletedAttack += OnHeroCompletedAttack;
        }

        private void RemoveListeners()
        {
            GameHeroSelectionController.OnHeroSelectedToAttack -= HandleOnHeroSelected;
            GameHeroSkillController.OnHeroCompletedAttack -= OnHeroCompletedAttack;
        }
    }
}
