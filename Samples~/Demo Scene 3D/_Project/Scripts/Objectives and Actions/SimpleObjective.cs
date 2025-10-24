using System;
using _Project.Scripts;
using AccessibleWayfinding;
using UnityEngine;
namespace _Project.Scripts
{
    public abstract class SimpleObjective : MonoBehaviour, IObjective
    {
        public event Action<IAction> OnObjectiveChanged;
        public string Description { get; protected set; }
        public IAction CurrentAction { get; protected set; }
        public bool IsCompleted { get; protected set; }
        public abstract void Activate();
        protected void Changed()
        {
            OnObjectiveChanged?.Invoke(CurrentAction);
        }
    }
}