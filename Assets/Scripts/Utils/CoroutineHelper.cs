using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Utils
{
    public static class CoroutineHelper
    {
        private static Dictionary<float, WaitForSeconds> _waitForSecondsDict = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds GetWaitForSeconds(float duration)
        {
            if (_waitForSecondsDict.ContainsKey(duration))
                return _waitForSecondsDict[duration];

            var waitForSeconds = new WaitForSeconds(duration);
            _waitForSecondsDict.Add(duration, waitForSeconds);
            return waitForSeconds;
        }
    }
}
