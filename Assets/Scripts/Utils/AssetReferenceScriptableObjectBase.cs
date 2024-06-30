using System;
using UnityEngine.AddressableAssets;

namespace RPGGame.Utils
{
    [Serializable]
    public class AssetReferenceScriptableObjectBase : AssetReferenceT<ScriptableObjectBase>
    {
        public AssetReferenceScriptableObjectBase(string guid) : base(guid)
        {
        }
    }

}