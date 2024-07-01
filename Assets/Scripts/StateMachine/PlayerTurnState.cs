using RPGGame.Game;
using System;

namespace RPGGame.StateMachine
{
    public class PlayerTurnState : BaseState
    {
        public static event Action OnEnablePlayerSelectionEvent;
        public static event Action OnDisablePlayerSelectionEvent;

        public PlayerTurnState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            AddListeners();
            OnEnablePlayerSelectionEvent?.Invoke();
        }
        public override void OnExit()
        {
            RemoveListeners();
            base.OnExit();
        }
        private void HandleOnHeroSelected(GameHero hero)
        {
            var target = _stateMachine.GetRandomGameHero(HeroTeam.Enemy);
            hero.Attack(target);
            _stateMachine.SwitchState(GameStates.EnemyTurn);
        }

     

        private void AddListeners()
        {
            GameHeroSelectionController.OnHeroSelectedToAttack += HandleOnHeroSelected;
        }

        private void RemoveListeners()
        {
            GameHeroSelectionController.OnHeroSelectedToAttack -= HandleOnHeroSelected;
        }
    }
}
