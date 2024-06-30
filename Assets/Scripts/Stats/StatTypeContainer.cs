using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Stats/StatContainer")]
    public class StatTypeContainer : ScriptableObject
    {
        public StatType[] StatTypes;

        [ContextMenu("Generate Stat Constants")]
        public void GenerateStatConstants()
        {
            StatConstantsGenerator.CreateScript(StatTypes);
        }
    }
}


