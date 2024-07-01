using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Level
{
    public class SpawnPoint : MonoBehaviour
    {
        private bool _isOccupied;
    
        public bool IsOccupied => _isOccupied;
        public void SetOccupied(bool value)
        {
            _isOccupied = value;
        }

        public Vector3 GetPosition() => transform.position;
    }
}
