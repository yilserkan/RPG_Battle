using RPGGame.CloudServices;
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
            _stateMachine.SaveGame();
            AddListeners();
            AttackRandomPlayer();
        }

        public override void OnExit()
        {
            base.OnExit();
            RemoveListeners();
        }
        private async void AttackRandomPlayer()
        {
            //var attacker = _stateMachine.GetRandomGameHero(HeroTeam.Enemy);
            //var receiver = _stateMachine.GetRandomGameHero(HeroTeam.Player);

            var response = await GameCloudRequests.SimulateEnemyAttack();

            if (response.IsSuccessfull)
            {
                var attackerHero= _stateMachine.GetGameHeroOfId(response.AttackerHeroID, HeroTeam.Enemy);
                var targetHero = _stateMachine.GetGameHeroOfId(response.ReceiverHeroID, HeroTeam.Player);
                attackerHero.SkillController.Attack(targetHero, response.Damage);
            }
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
