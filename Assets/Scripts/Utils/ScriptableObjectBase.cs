using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.Utils
{
    public class ScriptableObjectBase : ScriptableObject
    {
        public virtual Task Initialize() { return Task.CompletedTask; }
        public virtual Task Destroy() { return Task.CompletedTask; }
    }
}
