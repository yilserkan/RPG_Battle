using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Config
{
    public static class GameConfig 
    {
        private static GameConfigData _gameConfigData;

        public static GameConfigData Data => _gameConfigData;

        public static void InitializeGameConfig(GameConfigData gameConfigData)
        {
            _gameConfigData = gameConfigData;
        }
    }
}
