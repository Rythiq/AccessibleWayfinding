using UnityEngine;

namespace AccessibleWayfinding
{
    public interface ICue
    {
        enum CueType
        {
            Visual,
            Audio,
            Haptic
        }
        
        public CueType Type { get; }
        public bool IsActive { get; }
        public void Activate(Transform target);
        public void Deactivate();
    }
}