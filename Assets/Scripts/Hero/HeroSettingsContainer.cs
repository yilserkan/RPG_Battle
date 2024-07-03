using RPGGame.Game;
using RPGGame.Player;
using RPGGame.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.Hero
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Hero/SettingsContainer")]
    public class HeroSettingsContainer : ScriptableObjectBase
    {
        [SerializeField] private HeroSettings[] _heroSettings;

        private Dictionary<string, HeroSettings> _heroSettingsDict;

        public Dictionary<string, HeroSettings> HeroSettingsDict => _heroSettingsDict;

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
    }
}
