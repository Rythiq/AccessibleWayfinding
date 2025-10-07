using UnityEngine;

namespace AccessibleWayfinding
{
    public interface IWayfindingAccessibilitySettings
    {
        public bool EnabledVisualCues { get; set; }
        public bool EnabledAudioVisualisation { get; set; }
        public bool EnabledAudioCues { get; set; }
        public bool EnabledHapticCues { get; set; }
    }
}