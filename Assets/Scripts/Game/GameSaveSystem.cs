using RPGGame.Player;
using RPGGame.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Game
{
    public class GameSaveSystem
    {
        private JSONSaveSystem<GameData> _gameSaveSystem;
        private const string GAME_SAVE_SYSTEM_FILE_NAME = "GAMEDATA";

        public GameSaveSystem()
        {
            _gameSaveSystem = new JSONSaveSystem<GameData> (GAME_SAVE_SYSTEM_FILE_NAME);
        }

        public void Save()
        {
            var gameData = PlayerData.GetGameData();
            _gameSaveSystem.Save(gameData);
        }

        public void Load()
        {
            if(HasSaveFile())
            {
                _gameSaveSystem.Load(out GameData gameData);
                PlayerData.SetGameData(gameData);
            }
            else
            {
                PlayerData.CreateInitialGameData();
            }
        }

        public bool HasSaveFile()
        {
            return _gameSaveSystem.HasSaveFile();
        }
    }

    public class GameData
    {
        public int CurrentLevel;
        public int PlayerMatchCount;

        public GameData()
        {
            CurrentLevel = 0;
            PlayerMatchCount = 0;
        }
    }
}
