using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.StateMachine
{
    public abstract class BaseState
    {
        protected GameStateMachine _stateMachine;

        public BaseState(GameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
    }

    public class InitializationState : BaseState
    {
        public InitializationState(GameStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            InitializeGameState();
        }

        private void InitializeGameState()
        {

        }
    }
}