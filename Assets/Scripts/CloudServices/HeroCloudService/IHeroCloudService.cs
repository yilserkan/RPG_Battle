using RPGGame.Game;
using RPGGame.Hero;
using RPGGame.Player;
using RPGGame.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.CloudServices
{
    public interface IHeroCloudService
    {
        public Task<string> LoadHeroData();
        public Task<string> AddRandomHeroToPlayer();
    }
}

