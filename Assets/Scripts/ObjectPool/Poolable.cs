using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Pool
{
    public class Poolable : MonoBehaviour
    {
        [NonSerialized] public ObjectPoolSettings ObjectPoolSettings;
    }
}
