using AccessibleWayfinding;
using AccessibleWayfinding.AudioVisualizer;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Project.Scripts
{
    
    [RequireComponent(typeof(AudioSource))]
    public class AudioNavigationCue : NavigationCue
    {
        public float hintReachRadius;
        private Vector3 _nextPosition;
        private AudioSource _audioSource;
        public override ICue.CueType Type => ICue.CueType.Audio;
        private bool ReachedNextPosition => Vector3.Distance(Origin.position,_nextPosition) <= hintReachRadius;

        private bool OutOfRange => Vector3.Distance(Origin.position, transform.position) >= hintInterval*2;


        private void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
        }
        private void Update()
        {
            if (!IsActive)
            {
                _audioSource.mute = true;
                return;
            }
            _audioSource.mute = false;
            
            if (!Origin)
            {
                Origin = WayfindingManager.Instance.playerTransform;
                Debug.Log(WayfindingManager.Instance.playerTransform.position);
            }
            if ((Points == null)|| !(Points.Length > 0))
            {
                CalculatePath();
                transform.position = Points[0];
            }

            if (ReachedNextPosition || OutOfRange)
            {
                CalculatePath();
                _nextPosition = Points[0];
                int c = 1;
                while (ReachedNextPosition)
                {
                    if (c >= Points.Length)
                    {
                        return;
                    }
                    _nextPosition = Points[c];
                    c++;
                    if(c>20) return;
                }
            }
            transform.position = _nextPosition;
            
        }
        
        public void OnDrawGizmosSelected()
        {
            
            if(Origin && Target)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(Origin.position, 0.5f);
                Gizmos.DrawWireSphere(Origin.position, hintReachRadius);
                foreach (var point in Points)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(point, 0.5f);
                }
                Gizmos.color = Color.orange;
                Gizmos.DrawSphere(Target.position, 0.5f);
            }
        }
        
    }
}