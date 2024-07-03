using RPGGame.CloudServices;
using RPGGame.Game;
using System;
using static UnityEngine.GraphicsBuffer;

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
        private async void HandleOnHeroSelected(GameHero hero)
        {
            OnDisablePlayerSelectionEvent?.Invoke();

            //var target = _stateMachine.GetRandomGameHero(HeroTeam.Enemy);
            var response = await GameCloudRequests.AttackEnemyPlayer(hero.Hero.Settings.ID);
            if(response.IsSuccessfull)
            {
                var targetHero = _stateMachine.GetGameHeroOfId(response.EnemyId, HeroTeam.Enemy);
                hero.SkillController.Attack(targetHero, response.Damage);
            }
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
