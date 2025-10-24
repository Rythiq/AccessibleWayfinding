using AccessibleWayfinding;
using UnityEngine;

namespace _Project.Scripts
{
    public class WayfindingAccessibilitySettings : IWayfindingAccessibilitySettings
    {
        public bool EnabledVisualCues { get; set; }
        public Gradient PreferredGradient { get; set; }
        public bool EnabledAudioCues { get; set; }
        public bool EnabledAudioVisualisation { get; set; }
        public bool EnabledHapticCues { get; set; }
        
    }
}