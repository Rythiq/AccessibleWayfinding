using System;
using AccessibleWayfinding;
using UnityEngine;

namespace _Project.Scripts
{
    public abstract class SimpleAction : MonoBehaviour, IAction
    {
        public event Action OnActionCompleted;
        public string Description {get; protected set; }
        public Transform Target { get; protected set;}
        
        protected void Complete()
        {
            OnActionCompleted?.Invoke();
        }
    }
}