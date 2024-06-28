using RPGGame.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Initialization
{
    public class InitializationManager : MonoBehaviour
    {
        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            SceneLoader.Instance.LoadScene(SceneType.GameScene);
        }
    }
}