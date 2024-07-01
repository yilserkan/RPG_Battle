using RPGGame.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace RPGGame.Pool
{
    public class ObjectPool : AbstractMonoSingleton<ObjectPool>
    {
        private Dictionary<int, IObjectPool<Poolable>> _objectPools = new Dictionary<int, IObjectPool<Poolable>>();

        public static Poolable Spawn(ObjectPoolSettings flyweightSettings, Transform parent = null)
        {
            var pooled = Instance.GetPoolFor(flyweightSettings)?.Get();
            if(pooled.transform.parent != parent)
                pooled.transform.SetParent(parent);

            return pooled;
        }
        public static void ReturnToPool(Poolable flyweight) => Instance.GetPoolFor(flyweight.FlyweightSettings)?.Release(flyweight);

        IObjectPool<Poolable> GetPoolFor(ObjectPoolSettings flyweightSettings)
        {
            IObjectPool<Poolable> pool;

            if(_objectPools.TryGetValue(flyweightSettings.GetInstanceID(), out pool)) return pool;

            pool = new ObjectPool<Poolable>
                (
                   flyweightSettings.Create,
                   flyweightSettings.OnGet,
                   flyweightSettings.OnRelease,
                   flyweightSettings.OnDestroyPooledObject,
                   flyweightSettings.CollectionCheck,
                   flyweightSettings.DefaultCapacity,
                   flyweightSettings.MaxCapacity
                );

            _objectPools.Add(flyweightSettings.GetInstanceID(), pool);
            return pool;
        }
    }
}
