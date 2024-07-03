using UnityEngine;

namespace RPGGame.Config
{

    [CreateAssetMenu(menuName ="ScriptableObjects/Config/Settings")]
    public class GameConfigSettings : ScriptableObject
    {
        public GameConfigData GameConfigData;
    }
}
