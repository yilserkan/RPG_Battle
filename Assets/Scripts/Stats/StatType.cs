
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Stats/BaseStat")]
    public class StatType: ScriptableObject
    {
        public string ID;
        public string Name;
    }
}

