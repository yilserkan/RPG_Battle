using RPGGame.Game;
using System;

namespace RPGGame.StateMachine
{
    public class EnemyTurnState : BaseState
    {
        private const float _switchStateDelay = .5f;

        public EnemyTurnState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            AddListeners();
            AttackRandomPlayer();
        }

        public override void OnExit()
        {
            base.OnExit();
            RemoveListeners();
        }
        private void AttackRandomPlayer()
        {
            var attacker = _stateMachine.GetRandomGameHero(HeroTeam.Enemy);
            var receiver = _stateMachine.GetRandomGameHero(HeroTeam.Player);

            attacker.SkillController.Attack(receiver);
        }

        private void OnHeroCompletedAttack()
        {
            _stateMachine.DelayedSwitchState(GetNextState(), _switchStateDelay);
        }

        private GameStates GetNextState()
        {
            bool allPlayerHeroesDied = _stateMachine.CheckIfAllHeroesDiedInTeam(HeroTeam.Player);
            return allPlayerHeroesDied ? GameStates.GameResult : GameStates.PlayerTurn;
        }

        private void AddListeners()
        {
            GameHeroSkillController.OnHeroCompletedAttack += OnHeroCompletedAttack;
        }

        private void RemoveListeners()
        {
            GameHeroSkillController.OnHeroCompletedAttack -= OnHeroCompletedAttack;
        }
    }
}
