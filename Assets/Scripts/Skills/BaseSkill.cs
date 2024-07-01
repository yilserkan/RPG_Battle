using DG.Tweening;
using RPGGame.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Skills
{
    public abstract class BaseSkill : Poolable
    {
        protected AbstractSkillSettings _skillSettings;

        public void Initialize(AbstractSkillSettings skillSettings)
        {
            _skillSettings = skillSettings;
            ExecuteSkill();
        }

        public abstract void ExecuteSkill();
    }
}

