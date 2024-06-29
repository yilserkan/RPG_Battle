using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.SaveSystem
{
    public interface ISaveSystem<T>
    {
        public void Save(T data);
        public void Load(out T obj);
        public void Delete();
        public bool HasSaveFile();
    }
}