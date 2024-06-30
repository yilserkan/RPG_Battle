using RPGGame.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.Hero
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Hero/SettingsContainer")]
    public class HeroSettingsContainer : ScriptableObjectBase
    {
        [SerializeField] private HeroSettings[] _heroSettings;

        private Dictionary<string, HeroSettings> _heroSettingsDict;

        public override Task Initialize()
        {
            _heroSettingsDict = new Dictionary<string, HeroSettings>();

            for (int i = 0; i < _heroSettings.Length; i++)
            {
                if (!_heroSettingsDict.ContainsKey(_heroSettings[i].ID))
                {
                    _heroSettingsDict.Add(_heroSettings[i].ID, _heroSettings[i]);
                }
            }

            return Task.CompletedTask;
        }

        public override Task Destroy()
        {
            _heroSettingsDict.Clear();
            return Task.CompletedTask;
        }

        public bool TryGetHeroSettings(string heroId, out HeroSettings heroSettings)
        {
            if (_heroSettingsDict.ContainsKey(heroId))
            {
                heroSettings = _heroSettingsDict[heroId];
                return true;
            }

            heroSettings = null;   
            return false;
        }

        public HeroSettings[] GetRandomHeroes()
        { 
            //var heroSettings = new HeroSettings[3];
            //heroSettings[0] = _heroSettings[0];
            //heroSettings[1] = _heroSettings[1];
            //heroSettings[2] = _heroSettings[2];

            var heroSettings = new HeroSettings[_heroSettings.Length];
            for (int i = 0; i < _heroSettings.Length; i++)
            {
                heroSettings[i] = _heroSettings[i];
            }

            return heroSettings;
        }
    }
}
