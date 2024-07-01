using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Stats;
using RPGGame.Pool;

namespace RPGGame.Game
{
    public class GameHero : Pool.Poolable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private float _vitality;
        private Hero.Hero _hero;
        private HeroTeam _team;

        public Hero.Hero Hero => _hero;
        public float Vitality => _vitality;

        public void Initialize(Hero.Hero hero)
        {
            _team = (HeroTeam)hero.HeroTeam;
            _hero = hero;
            _vitality = _hero.Stats.GetAttributeValue(StatConstants.Vitality);
            _spriteRenderer.sprite = _hero.Settings.HeroSprite;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                ObjectPool.ReturnToPool(this);
            }
        }
    }

    public enum HeroTeam
    {
        Player,
        Enemy
    }
}
