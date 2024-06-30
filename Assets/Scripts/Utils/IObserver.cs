using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Utils
{
    public interface IObserver<T>
    {
        public void Notify(T value);
    }
}

