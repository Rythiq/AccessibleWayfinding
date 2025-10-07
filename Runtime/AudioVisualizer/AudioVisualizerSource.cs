using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace AccessibleWayfinding.AudioVisualizer{

    [RequireComponent(typeof(AudioSource))]
    public class AudioVisualizerSource : MonoBehaviour
    {
        private AudioSource _audioSource;
        public Sprite icon; // Icon to represent the audio source in the visualizer
        private bool _registeredInVisualizer;
        private float[] _samples = new float[64];
        private bool InVisualizerRange => _audioSource.maxDistance > 0f && Vector3.Distance(_audioSource.transform.position, AudioVisualizerUICue.Instance.transform.position) <= _audioSource.maxDistance;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_registeredInVisualizer && InVisualizerRange && _audioSource.isPlaying && AudioVisualizerUICue.Instance.IsActive)
            {
                // Register the audio source in the visualizer
                AudioVisualizerUICue.Instance.RegisterAudio(this);
                _registeredInVisualizer = true;
            }   
            else if (_registeredInVisualizer && (!InVisualizerRange || !_audioSource.isPlaying || !AudioVisualizerUICue.Instance.IsActive))
            {
                // Unregister the audio source from the visualizer
                AudioVisualizerUICue.Instance.UnregisterAudio(this);
                _registeredInVisualizer = false;
            }
        }

        private void OnDestroy()
        {
            AudioVisualizerUICue.Instance.UnregisterAudio(this);
        }

        public float GetLoudness()
        {
            if (!_audioSource.isPlaying || _audioSource.mute) return 0f;
            _audioSource.GetOutputData(_samples, 0);
            float sum = 0f;
            for (int i = 0; i < _samples.Length; i++)
            {
                sum += Mathf.Abs(_samples[i]);
            }
            _audioSource.GetOutputData(_samples, 1);
            for (int i = 0; i < _samples.Length; i++)
            {
                sum += Mathf.Abs(_samples[i]);
            }
            float loudness = Mathf.Sqrt(sum / (_samples.Length * 2));
            loudness = Mathf.Clamp01(loudness);
            // Get distance between AudioSource and AudioListener
            float distance = Vector3.Distance(_audioSource.transform.position, AudioVisualizerUICue.Instance.transform.position);

            // Get AudioSource properties
            float minDistance = _audioSource.minDistance;
            float maxDistance = _audioSource.maxDistance;
            AudioRolloffMode rolloffMode = _audioSource.rolloffMode;

            // Calculate attenuation based on rolloff mode
            float attenuation;
            switch (rolloffMode)
            {
                case AudioRolloffMode.Logarithmic:
                    // Logarithmic rolloff: volume decreases with 1/distance
                    attenuation = minDistance / Mathf.Max(minDistance, distance);
                    break;
                case AudioRolloffMode.Linear:
                    // Linear rolloff: volume decreases linearly from minDistance to maxDistance
                    attenuation = Mathf.Clamp01(1f - (distance - minDistance) / (maxDistance - minDistance));
                    break;
                case AudioRolloffMode.Custom:
                {
                    // Custom rolloff: Evaluate the AnimationCurve
                    AnimationCurve customCurve = _audioSource.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
                    float normalizedDistance = Mathf.Clamp((distance - minDistance) / (maxDistance - minDistance), 0f, 1f);
                    attenuation = customCurve.Evaluate(normalizedDistance);
                    break;
                }
                default:
                    // Default to no attenuation
                    attenuation = 1f;
                    break;
            }

            return attenuation/2 + loudness;
        }
    }
}