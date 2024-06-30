using RPGGame.Hero;
using RPGGame.Player;
using RPGGame.SaveSystem;
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
        [SerializeField] private HeroSettingsContainer _heroSettings;
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

            PlayerData.Initialize(_heroSettings);
            SaveSystemManager.Instance.LoadAllSystems();

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