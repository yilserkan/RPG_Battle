using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RPGGame.SceneManagement
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SceneData", fileName = "SceneData", order = 0)]
    public class SceneData : ScriptableObject
    {
        public AssetReference SceneReference;
        public SceneType SceneType;
        public AsyncOperationHandle OperationHandle;
    }
}