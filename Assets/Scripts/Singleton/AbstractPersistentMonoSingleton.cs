using RPGGame.Singleton;
using Unity.VisualScripting;
using UnityEngine;

namespace RPGGame.Singleton
{
    public abstract class AbstractPersistentMonoSingleton<T> : AbstractMonoSingleton<T> where T : MonoBehaviour
    {
        public override void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
