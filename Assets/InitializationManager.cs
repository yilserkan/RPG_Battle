using RPGGame.Hero;
using RPGGame.SceneManagement;
using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Initialization
{
    public class InitializationManager : MonoBehaviour
    {
        [SerializeField] private HeroCreator _heroCreator;
        [SerializeField] private ScriptableObjectBase[] _scriptableObjectBases;

        void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            for (int i = 0; i < _scriptableObjectBases.Length; i++)
            {
                await _scriptableObjectBases[i].Initialize();
            }

            _heroCreator.CreateHeroes();

            SceneLoader.Instance.LoadScene(SceneType.GameScene);
        }

        private void OnApplicationQuit()
        {
            for (int i = 0; i < _scriptableObjectBases.Length; i++)
            {
                _scriptableObjectBases[i].Destroy();
            }
        }
    }
}