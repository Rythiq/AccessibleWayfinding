using UnityEngine;

namespace AccessibleWayfinding
{
    public class PlayerSettings : ScriptableObject
    {
        bool enabledVisualCues = true;
        bool enabledAudioCues = true;
        bool enabledHapticCues = true;
        float fontScale = 1.0f;
        bool boldText = false;
        bool highContrastMode = false;
        bool screenReaderEnabled = false;
    }
}