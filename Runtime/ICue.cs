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
        public abstract CueType Type { get; }
        public bool IsActive { get; }
        public void Activate(Transform target);
        public void Deactivate();
    }
}