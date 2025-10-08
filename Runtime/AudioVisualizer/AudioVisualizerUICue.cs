using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AccessibleWayfinding.AudioVisualizer
{
    [RequireComponent(typeof(Camera))]
    public class AudioVisualizerUICue : MonoBehaviour, ICue
    {

        public bool IsGlobal => true;
        public ICue.CueType Type => ICue.CueType.None;
        public bool IsActive { get; private set;}
        public static AudioVisualizerUICue Instance { get; private set; }
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject iconPrefab;
        private List<AudioVisualizerSource> _sources = new List<AudioVisualizerSource>();
        private Dictionary<AudioVisualizerSource, Image> _icons = new Dictionary<AudioVisualizerSource, Image>();
        
        public void Activate(Transform target)
        {
			if(WayfindingManager.Instance.wayfindingAccessibilitySettings.EnabledAudioVisualisation)
                IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
            foreach (AudioVisualizerSource source in _sources)
            {
                if (_icons.ContainsKey(source))
                {
                    _icons[source].color = new Color(0, 0, 0, 0);
                }
            }
        }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            IsActive = true;
        }
        
        public void RegisterAudio(AudioVisualizerSource audioVisualizerSource)
        {
            if(!_sources.Contains(audioVisualizerSource))
            {
                _sources.Add(audioVisualizerSource);
                CreateIcon(audioVisualizerSource);
            }
        }
        
        public void UnregisterAudio(AudioVisualizerSource audioVisualizerSource)
        {
            if (_sources.Contains(audioVisualizerSource))
            {
                _sources.Remove(audioVisualizerSource);
                DestroyIcon(audioVisualizerSource);
            }
        }
        
        private void CreateIcon(AudioVisualizerSource source)
        {
            GameObject iconObject = Instantiate(iconPrefab, canvas.transform);
            _icons.Add(source, iconObject.GetComponent<Image>());
            _icons[source].color = new Color(0, 0, 0, 0);
            _icons[source].sprite = source.icon; 
            iconObject.transform.SetParent(canvas.transform, false);
            
        }

        private void DestroyIcon(AudioVisualizerSource source)
        {
            if (_icons.ContainsKey(source))
            {
                Destroy(_icons[source].gameObject);
                _icons.Remove(source);
            }
        }

        private void Update()
        {
            if(!IsActive || _sources.Count == 0)
            {
                return;
            }
            foreach (var source in _sources)
            {
                if (_icons.ContainsKey(source))
                {
                    var direction = Camera.main.worldToCameraMatrix * (source.transform.position - transform.position);
                    _icons[source].rectTransform.anchoredPosition = new Vector2(direction.x, -direction.z +direction.y).normalized*200f;
                    float attenuation = source.GetLoudness();
                    _icons[source].color = new Color(1f, 1f, 1f, attenuation);
                }
            }
        }
    }
}