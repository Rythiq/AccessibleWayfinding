using System.Collections.Generic;
using AccessibleWayfinding;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts
{
    public abstract class NavigationCue : MonoBehaviour, ICue
    {
        
        private NavMeshPath _path;
        protected Transform Target;
        protected Vector3[] Points;
        protected Transform Origin;
        public bool IsGlobal => true;
        public abstract ICue.CueType Type { get; }
        public bool IsActive { get; protected set; }
        
        [SerializeField] public float hintInterval;
        public virtual void Activate(Transform target)
        {
            Target = target;
            IsActive = true;
        }
        public virtual void Deactivate()
        {
            IsActive = false;
        }
        private void Start()
        {
            _path = new NavMeshPath();
            WayfindingManager.Instance.RegisterCue(this);
            Debug.Log("Cue registered");
        }

        protected void CalculatePath()
        {
            if(NavMesh.CalculatePath(Origin.position, Target.position, NavMesh.AllAreas, _path))
            {
                List<Vector3> pp = new List<Vector3>();
                for (int i = 0; i < _path.corners.Length - 1; ++i)
                {
                    var point = _path.corners[i];
                    var point2 = _path.corners[i + 1];
                    while (Vector3.Distance(point, point2) > hintInterval)
                    {
                        pp.Add(point);
                        point += (point2 - point).normalized*hintInterval;
                    }
                    pp.Add(point);
                }
                pp.RemoveAt(0);
                pp.Add(Target.position);
                Points = pp.ToArray();
            }
            else
            {
                Debug.LogWarning("Failed to calculate path from " + Origin.position + " to " + Target.position);
            }
        }
    }
}