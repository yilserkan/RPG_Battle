using RPGGame.Game;
using System;

namespace RPGGame.StateMachine
{
    public class PlayerTurnState : BaseState
    {
        public static event Action OnEnablePlayerSelectionEvent;
        public static event Action OnDisablePlayerSelectionEvent;

        private bool _hasPlayerSelectedHero;
        private const float _switchStateDelay = .5f;

        public PlayerTurnState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            AddListeners();
            _hasPlayerSelectedHero = false;
            OnEnablePlayerSelectionEvent?.Invoke();
        }
        public override void OnExit()
        {
            base.OnExit();
            RemoveListeners();
        }
        private void HandleOnHeroSelected(GameHero hero)
        {
            if (_hasPlayerSelectedHero) { return; }
            _hasPlayerSelectedHero = true;

            var target = _stateMachine.GetRandomGameHero(HeroTeam.Enemy);
            hero.Attack(target);
        }

        private void OnHeroCompletedAttack()
        {
            _stateMachine.DelayedSwitchState(GameStates.EnemyTurn, _switchStateDelay);
        }

        private void AddListeners()
        {
            GameHeroSelectionController.OnHeroSelectedToAttack += HandleOnHeroSelected;
            GameHero.OnHeroCompletedAttack += OnHeroCompletedAttack;
        }

        private void RemoveListeners()
        {
            GameHeroSelectionController.OnHeroSelectedToAttack -= HandleOnHeroSelected;
            GameHero.OnHeroCompletedAttack -= OnHeroCompletedAttack;
        }
    }
}
