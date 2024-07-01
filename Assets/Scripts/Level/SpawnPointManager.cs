using RPGGame.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Level
{
    public class SpawnPointManager : MonoBehaviour
    {
        [SerializeField] private SpawnPointData[] _playerSpawnPoints;

        private Dictionary<HeroTeam, SpawnPointData> _spawPointDataDict;

        private void Start()
        {
            _spawPointDataDict = new Dictionary<HeroTeam, SpawnPointData>();

            for (int i = 0; i < _playerSpawnPoints.Length; i++)
            {
                if (!_spawPointDataDict.ContainsKey(_playerSpawnPoints[i].HeroTeam))
                {
                    _spawPointDataDict.Add(_playerSpawnPoints[i].HeroTeam, _playerSpawnPoints[i]);
                }
            }
        }

        public Vector3 GetSpawnPoint(HeroTeam team)
        {
            if (!_spawPointDataDict.ContainsKey(team)) return Vector3.zero;

            var spawnPoints = _spawPointDataDict[team].SpawnPoints;
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if (!spawnPoints[i].IsOccupied)
                {

                    spawnPoints[i].SetOccupied(true);
                    return spawnPoints[i].GetPosition();
                }
            }

            return Vector3.zero;
        }
    }

    public class SpawnPointData
    {
        public HeroTeam HeroTeam;
        public SpawnPoint[] SpawnPoints;
    }

}
