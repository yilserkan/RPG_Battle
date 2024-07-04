using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPGGame.Stats
{
    public class StatInfo : MonoBehaviour
    {
        [SerializeField] private StatTypeSettings type;
        [SerializeField] private TextMeshProUGUI _statNameText;
        [SerializeField] private TextMeshProUGUI _statValueText;

        public StatTypeSettings Type => type;

        public void Setup(float attrValue)
        {
            //_statNameText.text = type.Name;
            _statValueText.text = $"{Mathf.CeilToInt(attrValue)}";
        }
    }
}
