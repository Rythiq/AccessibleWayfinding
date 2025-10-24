using System;
using _Project.Scripts;
using AccessibleWayfinding;
using UnityEngine;
    public class SingleActionObjective : SimpleObjective
    {
        public string description;
        public SimpleAction action;
        public bool assignSelfOnStart = false;
        public SingleActionObjective nextObjective;
        private void Start()
        {
            Description = description;
            CurrentAction = action;
            if (!assignSelfOnStart) return;
            WayfindingManager.Instance.SetObjective(this);
        }
        public override void Activate()
        {
            CurrentAction.OnActionCompleted += () =>
            {
                IsCompleted = true;
                Changed();
                if (nextObjective != null)
                {
                    WayfindingManager.Instance.SetObjective(nextObjective);
                }
            };
        }
    }
