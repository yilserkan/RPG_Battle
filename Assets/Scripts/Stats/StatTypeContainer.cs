using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Stats/StatContainer")]
    public class StatTypeContainer : ScriptableObject
    {
        public StatTypeSettings[] StatTypes;
    }
}


