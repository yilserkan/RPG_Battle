using UnityEngine;

namespace RPGGame.SaveSystem
{
    public class JSONSerializer<T> : ISerializer<T>
    {
        public virtual string Serialize(T data)
        {
            return JsonUtility.ToJson(data);
        }

        public virtual void Deserialize(string json, out T obj)
        {
            obj = JsonUtility.FromJson<T>(json);
        }
    }
}