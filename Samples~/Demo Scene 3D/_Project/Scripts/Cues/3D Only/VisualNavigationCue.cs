using System;
using System.Collections.Generic;
using AccessibleWayfinding;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.WSA;

namespace _Project.Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class VisualNavigationCue : NavigationCue
    {
        public float hintReachRadius;
        private LineRenderer _lineRenderer;
        public override ICue.CueType Type => ICue.CueType.Visual;
        
        Vector3 nextPosition => Points != null && Points.Length > 0 ? Points[0] : Vector3.negativeInfinity;
        private bool ReachedNextPosition => Vector3.Distance(Origin.position,nextPosition) <= hintReachRadius;
        
        private bool OutOfRange => Vector3.Distance(Origin.position, nextPosition) > hintInterval * 2;
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            Origin = transform;
            if (_lineRenderer == null)
            {
                Debug.LogError("LineRenderer component is missing on " + gameObject.name);
            }
        }
        private void Update()
        {
            if (IsActive)
            {
                if (!_lineRenderer.enabled)
                {
                    CalculatePath();
                }
                if (!ReachedNextPosition && !OutOfRange)
                {
                    DrawLine();
                }
                else
                {
                    CalculatePath();
                }
            }
            else
            {
                _lineRenderer.enabled = false;
            }
        }

        private void DrawLine()
        {
            
            if (_lineRenderer && Points != null && Points.Length > 0)
            {
                Gradient gradient = SettingsManager.Instance.getSettings().PreferredGradient;
                _lineRenderer.colorGradient = gradient;
                _lineRenderer.enabled = true;
                _lineRenderer.positionCount = Points.Length + 1;
                List<Vector3> points = new List<Vector3>(Points);
                points.Insert(0, Origin.position);
                _lineRenderer.SetPositions(points.ToArray());
            }
            else
            {
                Debug.LogWarning("LineRenderer or Points are not set correctly.");
            }
        }

        public void OnDrawGizmosSelected()
        {
            foreach (var point in Points)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(point, 0.5f);
            }
        }
    }
}