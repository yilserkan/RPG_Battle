namespace RPGGame.Game
{
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
