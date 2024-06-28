using System;
using System.Threading.Tasks;
using RPGGame.Utils;
using RPGGame.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using RPGGame.Singleton;

namespace RPGGame.SceneManagement
{
    public class SceneLoader : AbstractMonoSingleton<SceneLoader>
    {
        [SerializeField] private SceneLoaderUIHelper _uiHelper;
        [SerializeField] private AssetReferenceScriptableObject[] _sceneDatas;

        private AsyncOperationHandle<SceneInstance> _activeAddressableSceneHandle;
        private AsyncOperationHandle<SceneInstance> _prevAddressableSceneHandle;

        private const string INITIALIZATION_SCENE_NAME = "InitializationScene";

        [ContextMenu("LoadGameScene")]
        public void LoadGameScene()
        {
            LoadScene(SceneType.GameScene);
        }

        [ContextMenu("LoadInitialScene")]
        public void LoadInitialScene()
        {
            LoadScene(SceneType.InitializationScene);
        }

        public async Task LoadScene(SceneType type)
        {
            _uiHelper.EnableLoadingPanel(true);

            Scene prevScene = SceneManager.GetActiveScene();
            _prevAddressableSceneHandle = _activeAddressableSceneHandle;

            if (type == SceneType.InitializationScene)
            {
                await LoadInitialScene(_uiHelper);
            }
            else
            {
                await LoadAddressableScene(type, _uiHelper);
            }

            await Task.Delay(1000);
            await UnloadScene(prevScene);

            _uiHelper.EnableLoadingPanel(false);
        }

        private async Task LoadInitialScene(IProgress<float> progress)
        {
            var handle = SceneManager.LoadSceneAsync(INITIALIZATION_SCENE_NAME, LoadSceneMode.Additive);

            while (!handle.isDone)
            {
                progress?.Report(handle.progress);
                await Task.Delay(100);
            }
        }

        private async Task LoadAddressableScene(SceneType type, IProgress<float> progress)
        {
            var sceneReference = await GetAddressableSceneReference(type);
            if (sceneReference == null) { return; }

            _activeAddressableSceneHandle = sceneReference.LoadSceneAsync(LoadSceneMode.Additive);

            while (!_activeAddressableSceneHandle.IsDone)
            {
                progress?.Report(_activeAddressableSceneHandle.PercentComplete);
                await Task.Delay(100);
            }
        }

        public async Task UnloadScene(Scene scene)
        {
            if (scene.name.Equals(INITIALIZATION_SCENE_NAME))
            {
                await UnloadInitialScene();
            }
            else
            {
                await UnloadAddressableScene();
            }
        }

        private async Task UnloadInitialScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            var handle = SceneManager.UnloadSceneAsync(activeScene);

            while (!handle.isDone)
            {
                await Task.Delay(100);
            }

            Resources.UnloadUnusedAssets();
        }

        private async Task UnloadAddressableScene()
        {
            if (!_prevAddressableSceneHandle.IsValid()) { return; }

            var handle = Addressables.UnloadSceneAsync(_prevAddressableSceneHandle);
            while (!handle.IsDone)
            {
                await Task.Delay(100);
            }

            Resources.UnloadUnusedAssets();
        }

        private async Task<AssetReference> GetAddressableSceneReference(SceneType type)
        {
            for (int i = 0; i < _sceneDatas.Length; i++)
            {
                var sceneData = await _sceneDatas[i].LoadAddressableAsync() as SceneData;
                if (sceneData != null && sceneData.SceneType == type)
                {
                    return sceneData.SceneReference;
                }
            }

            return null;
        }
    }

    public enum SceneType { InitializationScene, GameScene }
}
