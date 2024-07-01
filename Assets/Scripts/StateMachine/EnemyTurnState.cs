using RPGGame.Game;

namespace RPGGame.StateMachine
{
    public class EnemyTurnState : BaseState
    {
        public EnemyTurnState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            AttackRandomPlayer();
            _stateMachine.SwitchState(GameStates.PlayerTurn);
        }

        private void AttackRandomPlayer()
        {
            var attacker = _stateMachine.GetRandomGameHero(HeroTeam.Enemy);
            var receiver = _stateMachine.GetRandomGameHero(HeroTeam.Player);

            attacker.Attack(receiver);
        }
    }
}
