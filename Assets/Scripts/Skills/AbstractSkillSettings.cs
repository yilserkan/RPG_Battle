using RPGGame.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Skills
{
    public abstract class AbstractSkillSettings : ScriptableObject
    {
        public string AnimationName;

        public abstract void ExecuteSkill(GameHero attacker, GameHero receiver);
    }

    public class ProjectileSkillSettings : AbstractSkillSettings
    {
        public BaseSkill SkillPrefab;

        public override void ExecuteSkill(GameHero attacker, GameHero receiver)
        {
            
        }
    }
}
