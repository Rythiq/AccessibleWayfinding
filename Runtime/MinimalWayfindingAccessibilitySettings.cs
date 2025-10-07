using UnityEngine;

namespace AccessibleWayfinding
{
    public class MinimalWayfindingAccessibilitySettings : IWayfindingAccessibilitySettings
    {
        public bool EnabledVisualCues { get; set; }
        public bool EnabledAudioCues { get; set; }
        public bool EnabledAudioVisualisation { get; set; }
        public bool EnabledHapticCues { get; set; }
    }
}