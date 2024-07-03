using RPGGame.CloudServices;
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
        [SerializeField] private AssetReferenceScriptableObject _heroSettings;
        [SerializeField] private AssetReferenceScriptableObjectBase[] _scriptableObjectBases;

        ScriptableObject[] _scriptableObjectScripts;

        void Start()
        {
            Initialize();
        }

        private async void Initialize()
        {
            for (int i = 0; i < _scriptableObjectBases.Length; i++)
            {
                var asset = await _scriptableObjectBases[i].LoadAssetAsync().Task;
                await asset.Initialize();
            }

            var heroSettings = await _heroSettings.LoadAssetAsync().Task;
            PlayerData.Initialize((HeroSettingsContainer)heroSettings);


            //SaveSystemManager.Instance.LoadAllSystems();
            var response = await GameCloudRequests.LoadGameData();
            PlayerData.SetGameData(response.GameData);

            //var heroResponse = await HeroCloudRequests.LoadHeroData();
            await PlayerData.SetPlayerHeroes();

            SceneLoader.Instance.LoadScene(SceneType.GameScene);
        }

        private void OnApplicationQuit()
        {
            for (int i = 0; i < _scriptableObjectBases.Length; i++)
            {
                //_scriptableObjectBases[i].Destroy();
            }
        }

    }
}