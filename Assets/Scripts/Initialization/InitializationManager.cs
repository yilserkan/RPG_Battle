using RPGGame.CloudServices;
using RPGGame.Config;
using RPGGame.Hero;
using RPGGame.Player;
using RPGGame.SaveSystem;
using RPGGame.SceneManagement;
using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.Initialization
{
    public class InitializationManager : MonoBehaviour
    {
        [SerializeField] private GameConfigSettings _gameConfigSettings;
        [SerializeField] private AssetReferenceScriptableObject _heroSettings;
        [SerializeField] private AssetReferenceScriptableObjectBase[] _scriptableObjectBases;

        void Start()
        {
            Application.targetFrameRate = 60;
            Initialize();
        }

        private void OnApplicationQuit()
        {
            DestroyScriptableObjects();
        }

        private async void Initialize()
        {
            await InitializeScriptableObjects();

            GameConfig.InitializeGameConfig(_gameConfigSettings.GameConfigData);

            var heroSettings = await _heroSettings.LoadAssetAsync().Task;
            PlayerData.Initialize((HeroSettingsContainer)heroSettings);

            var response = await GameCloudRequests.LoadGameData();
            PlayerData.SetGameData(response.GameData);

            await PlayerData.SetPlayerHeroes();

            await SceneLoader.Instance.LoadScene(SceneType.GameScene);
        }

        private async Task InitializeScriptableObjects()
        {
            for (int i = 0; i < _scriptableObjectBases.Length; i++)
            {
                var asset = await _scriptableObjectBases[i].LoadAssetAsync().Task;
                await asset.Initialize();
                //_scriptableObjectBases[i].ReleaseAsset();
            }
        }

        private async void DestroyScriptableObjects()
        {
            for (int i = 0; i < _scriptableObjectBases.Length; i++)
            {
                var asset = await _scriptableObjectBases[i].LoadAssetAsync().Task;
                await asset.Destroy();
                _scriptableObjectBases[i].ReleaseAsset();
            }
        }
    }
}