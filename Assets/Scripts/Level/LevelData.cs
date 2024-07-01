using RPGGame.Hero;
using System;
using RPGGame.StateMachine;

namespace RPGGame.Game
{
    [Serializable]
    public class LevelData
    {
        public GameHeroData[] PlayerHeroes;
        public GameHeroData[] EnemyHeroes;
        public int Level;
        public int CurrentState;

        public LevelData(GameHeroData[] playerHeroes, GameHeroData[] enemyHeroes, int level)
        {
            PlayerHeroes = playerHeroes;
            EnemyHeroes = enemyHeroes;
            Level = level;
            CurrentState = (int)GameStates.None;
        }
    }
}
