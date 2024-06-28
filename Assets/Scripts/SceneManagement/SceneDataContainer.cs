using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Utils;
using RPGGame.Extensions;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

namespace RPGGame.SceneManagement
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SceneDataContainer", fileName = "SceneData", order = 0)]
    public class SceneDataContainer : ScriptableObjectBase
    {
        [SerializeField] private AssetReferenceScriptableObject[] _sceneDatas;
        private SceneData[] _loadedSceneDatas;

        public SceneData[] LoadedSceneDatas { get => _loadedSceneDatas; }

        public override async Task Initialize()
        {
            var sceneCount = _sceneDatas.Length;
            _loadedSceneDatas = new SceneData[sceneCount];
            for (int i = 0; i < _sceneDatas.Length; i++)
            {
                var handle = await _sceneDatas[i].LoadAddressableAsync();
                var sceneData = handle.Result as SceneData;
                sceneData.OperationHandle = handle;
                _loadedSceneDatas[i] = sceneData;
            }
        }

        public override Task Destroy()
        {
            if (_loadedSceneDatas == null) { return Task.CompletedTask; }
            for (int i = 0; i < _loadedSceneDatas.Length; i++)
            {
                Addressables.ReleaseInstance(_loadedSceneDatas[i].OperationHandle);
            }
            return Task.CompletedTask;
        }

        public async Task<AssetReference> GetAddressableSceneReference(SceneType type)
        {
            for (int i = 0; i < _loadedSceneDatas.Length; i++)
            {
                var sceneData = _loadedSceneDatas[i];
                if (sceneData != null && sceneData.SceneType == type)
                {
                    return sceneData.SceneReference;
                }
            }

            return null;
        }
    }
}

