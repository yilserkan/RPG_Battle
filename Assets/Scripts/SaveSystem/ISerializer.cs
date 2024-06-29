using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.SaveSystem
{
    public interface ISerializer<T>
    {
        public string Serialize(T data);
        public void Deserialize(string json, out T obj);
    }
}