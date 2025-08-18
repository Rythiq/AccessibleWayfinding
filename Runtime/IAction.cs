using UnityEngine;

namespace AccessibleWayfinding
{
    public interface IAction
    {
        public string Description { get; }
        public Transform Target { get; }
        public bool IsCompleted { get; }
        
        public void Complete();
    }
}