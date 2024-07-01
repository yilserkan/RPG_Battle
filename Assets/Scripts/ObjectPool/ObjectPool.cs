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

        public static Poolable Spawn(ObjectPoolSettings poolableSettings, Transform parent = null)
        {
            var pooled = Instance.GetPoolFor(poolableSettings)?.Get();
            if(pooled.transform.parent != parent)
                pooled.transform.SetParent(parent);

            return pooled;
        }
        public static void ReturnToPool(Poolable poolabe) => Instance.GetPoolFor(poolabe.ObjectPoolSettings)?.Release(poolabe);

        IObjectPool<Poolable> GetPoolFor(ObjectPoolSettings poolableSettings)
        {
            IObjectPool<Poolable> pool;

            if(_objectPools.TryGetValue(poolableSettings.GetInstanceID(), out pool)) return pool;

            pool = new ObjectPool<Poolable>
                (
                   poolableSettings.Create,
                   poolableSettings.OnGet,
                   poolableSettings.OnRelease,
                   poolableSettings.OnDestroyPooledObject,
                   poolableSettings.CollectionCheck,
                   poolableSettings.DefaultCapacity,
                   poolableSettings.MaxCapacity
                );

            _objectPools.Add(poolableSettings.GetInstanceID(), pool);
            return pool;
        }
    }
}
