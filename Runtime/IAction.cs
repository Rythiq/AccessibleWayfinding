using System;
using UnityEngine;

namespace AccessibleWayfinding
{
    public interface IAction
    {
        public event Action OnActionCompleted;
        public string Description { get; }
        public Transform Target { get; }
        public bool IsCompleted { get; }
        
        public void Complete();
    }
}