using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Pool
{
    [CreateAssetMenu(menuName ="ScriptableObjects/ObjectPool/Settings")]
    public class ObjectPoolSettings : ScriptableObject
    {
        public Poolable Prefab;
        public bool CollectionCheck = true;
        public int DefaultCapacity = 10;
        public int MaxCapacity = 1000;

        public Poolable Create()
        {
            var instantiated = Instantiate(Prefab);
            instantiated.gameObject.SetActive(true);
            instantiated.ObjectPoolSettings = this;

            return instantiated;
        }

        public void OnGet(Poolable flyweight) => flyweight.gameObject.SetActive(true);
        public void OnRelease(Poolable flyweight) => flyweight.gameObject.SetActive(false);
        public void OnDestroyPooledObject(Poolable flyweight) => Destroy(flyweight.gameObject);
    }
}
