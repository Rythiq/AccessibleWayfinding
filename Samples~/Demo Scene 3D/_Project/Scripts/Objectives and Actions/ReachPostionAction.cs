using System;
using AccessibleWayfinding;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class ReachPostionAction : SimpleAction
    {
        public bool isCompleted;
        public float radius = 1.5f;
        public string description;

        private void Awake()
        {
            Description = description;
            isCompleted = false;
            Target = transform;
        }
        private void Update()
        {
            if (isCompleted) return;
            if ((WayfindingManager.Instance.playerTransform.position - Target.position).magnitude <= radius)
            {
                isCompleted = true;
                Complete();
            }
        }
    }
}