using System;

namespace RPGGame.Config
{
    [Serializable]
    public class GameConfigData
    {
        public int StartHeroCount= 3;
        public int ReceiveNewHeroInterval= 2;
        public int LevelIncreaseInterval = 3;
    }
}
