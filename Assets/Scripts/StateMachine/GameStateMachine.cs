using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.StateMachine
{
    public class GameStateMachine : MonoBehaviour
    {
        private BaseState _currentState;

        private Dictionary<GameStates, BaseState> _states =
            new Dictionary<GameStates, BaseState>()
            {
                //{ GameStates.None, new BaseState() }
            };

        public void SwitchState(GameStates newState)
        {
            _currentState?.OnExit();
            _currentState = _states[newState];
            _currentState?.OnEnter();
        }

        private void OnStateEnter()
        {
            _currentState?.OnEnter();
        }

        private void OnStateExit()
        {
            _currentState?.OnExit();
        }

        private void OnStateUpdate()
        {
            _currentState?.OnUpdate();
        }

    }

    public enum GameStates
    {
        None,

    }
}

