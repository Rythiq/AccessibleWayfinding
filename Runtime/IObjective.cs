using System.Collections.Generic;

namespace AccessibleWayfinding
{
    public interface IObjective
    {
        public string Description { get; }
        public IEnumerable<IAction> Actions { get; }
        public IAction CurrentAction { get; }
        public bool isCompleted { get; }
        
        public void Activate();
    }
}