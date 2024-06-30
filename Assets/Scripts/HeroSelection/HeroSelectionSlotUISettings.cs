using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.HeroSelection
{
    [CreateAssetMenu(menuName = "ScriptableObjects/HeroSelection/UISettings")]
    public class HeroSelectionSlotUISettings : ScriptableObject
    {
        public Color SlotDefaultColor;
        public Color SlotUnselectableColor;

        public Color FrameSelectedColor;
        public Color FrameUnselectedColor;
    }
}

