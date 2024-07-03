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
        public int CurrentState;

        public LevelData()
        {
            PlayerHeroes = new GameHeroData[0];
            EnemyHeroes = new GameHeroData[0];
            CurrentState = (int)GameStates.None;
        }

        public LevelData(GameHeroData[] playerHeroes, GameHeroData[] enemyHeroes, GameStates currentState = GameStates.None)
        {
            PlayerHeroes = playerHeroes;
            EnemyHeroes = enemyHeroes;
            CurrentState = (int)currentState;
        }
    }
}
