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
        public Sprite icon;
        private bool _registeredInVisualizer;
        private float[] _samples = new float[256];
        private bool InVisualizerRange => _audioSource.maxDistance > 0f && Vector3.Distance(_audioSource.transform.position, AudioVisualizerUICue.Instance.transform.position) <= _audioSource.maxDistance;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (!_registeredInVisualizer && InVisualizerRange && _audioSource.isPlaying && AudioVisualizerUICue.Instance.IsActive)
            {
                AudioVisualizerUICue.Instance.RegisterAudio(this);
                _registeredInVisualizer = true;
            }   
            else if (_registeredInVisualizer && (!InVisualizerRange || !_audioSource.isPlaying || !AudioVisualizerUICue.Instance.IsActive))
            {
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
                sum += _samples[i] * _samples[i];
            }
            _audioSource.GetOutputData(_samples, 1);
            for (int i = 0; i < _samples.Length; i++)
            {
                sum += _samples[i] * _samples[i];
            }
            float loudness = Mathf.Sqrt(sum / _samples.Length);
            return loudness * 3f;
        }
    }
}
