using RPGGame.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Skills
{
    public abstract class AbstractSkillSettings : ScriptableObject
    {
        public string AnimationName;

        protected GameHero _attacker;
        protected GameHero _receiver;

        public GameHero Attacker => _attacker;
        public GameHero Receiver => _receiver;

        public abstract void ExecuteSkill(GameHero attacker, GameHero receiver);
    }
}
