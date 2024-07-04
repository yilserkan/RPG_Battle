using RPGGame.Player;
using RPGGame.SaveSystem;
using System.Collections;
using System.Collections.Generic;

namespace RPGGame.Game
{
    public class GameSaveSystem
    {
        private ISaveSystem<GameData> _gameSaveSystem;
        private const string GAME_SAVE_SYSTEM_FILE_NAME = "GAMEDATA";

        public GameSaveSystem()
        {
            _gameSaveSystem = new JSONSaveSystem<GameData> (GAME_SAVE_SYSTEM_FILE_NAME);
        }

        public void Save(GameData gameData)
        {
            _gameSaveSystem.Save(gameData);
        }

        public GameData Load()
        {
            _gameSaveSystem.Load(out GameData gameData);
            return gameData;
        }

        public bool HasSaveFile()
        {
            return _gameSaveSystem.HasSaveFile();
        }
    }
}
