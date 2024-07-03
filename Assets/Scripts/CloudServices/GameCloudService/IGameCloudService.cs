using RPGGame.Game;
using RPGGame.Hero;
using RPGGame.Player;
using RPGGame.SaveSystem;
using RPGGame.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.CloudServices
{
    public interface IGameCloudService 
    {
        public Task<string> CreateLevelData(string[] selectedHeroIds);
        public Task<string> AttackEnemyPlayer(string selectedPlayerHero);
        public Task<string> SimulateEnemyAttack();
    }
}
