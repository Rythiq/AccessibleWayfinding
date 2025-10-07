using System;
using System.Collections.Generic;
using UnityEngine;

namespace AccessibleWayfinding
{
    public interface IObjective
    {
        public event Action<IAction> OnObjectiveChanged;
        public string Description { get; }
        public IEnumerable<IAction> Actions { get; }
        public IAction CurrentAction { get; }
        public bool IsCompleted { get; }
        public void Activate();
    }
}