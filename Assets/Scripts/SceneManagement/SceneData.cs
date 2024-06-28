using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RPGGame.SceneManagement
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SceneData", fileName = "SceneData", order = 0)]
    public class SceneData : ScriptableObject
    {
        public AssetReference SceneReference;
        public SceneType SceneType;
    }
}