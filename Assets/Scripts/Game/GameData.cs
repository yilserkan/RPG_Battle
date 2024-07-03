using System;

namespace RPGGame.Game
{
    [Serializable]
    public class GameData
    {
        public int PlayerMatchCount;
        public LevelData ActiveLevelData;

        public GameData()
        {
            PlayerMatchCount = 0;
            ActiveLevelData = null;
        }
    }
}
