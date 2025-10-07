using System.Collections.Generic;
using UnityEngine;

namespace AccessibleWayfinding

{
    public class WayfindingManager : MonoBehaviour
    {
        public static WayfindingManager Instance { get; private set; }
        public IObjective CurrentObjective { get; private set; }
        private IWayfindingAccessibilitySettings _wayfindingAccessibilitySettings { get; set; }

        [SerializeField] public Transform playerTransform;
        
        private List<ICue> _cues = new List<ICue>();
        
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
        }

        public void RegisterCue(ICue cue)
        {
            _cues.Add(cue);
            if (_wayfindingAccessibilitySettings == null)
                _wayfindingAccessibilitySettings = new MinimalWayfindingAccessibilitySettings
                {
                    EnabledAudioCues = true,
                    EnabledVisualCues = true,
                    EnabledHapticCues = true,
                    EnabledAudioVisualisation = true,
                };
            UpdateCuesForCurrentAction(CurrentObjective.CurrentAction);
        }
        
        public void SetObjective(IObjective objective)
        {
            if(CurrentObjective != null)
            {
                CurrentObjective.OnObjectiveChanged -= UpdateCuesForCurrentAction;
            }
            CurrentObjective = objective;
            CurrentObjective.Activate();
            CurrentObjective.OnObjectiveChanged += UpdateCuesForCurrentAction;
            UpdateCuesForCurrentAction(CurrentObjective.CurrentAction);
        }
        public void SetPreferences(IWayfindingAccessibilitySettings preferences)
        {
            _wayfindingAccessibilitySettings = preferences;
            UpdateCuesForCurrentAction(CurrentObjective.CurrentAction);
        }
        private void UpdateCuesForCurrentAction(IAction action)
        {
            if(CurrentObjective.IsCompleted)
            {
                foreach (var cue in _cues)
                {
                    cue.Deactivate();
                }
                return;
            }
            
            foreach (ICue cue in _cues)
            {
                switch (cue.Type)
                {
                    case ICue.CueType.Audio:
                        if (_wayfindingAccessibilitySettings.EnabledAudioCues)
                        {
                            Debug.Log("Audio cue enabled" + cue);
                            cue.Activate(action.Target);
                        }else
                        {
                            Debug.Log("Audio cue disabled" + cue);
                            cue.Deactivate();
                        }
                        break;
                    case ICue.CueType.Visual:
                        if (_wayfindingAccessibilitySettings.EnabledVisualCues)
                        {
                            Debug.Log("Visual cue enabled");
                            cue.Activate(action.Target);
                        } else
                        {
                            cue.Deactivate();
                        }
                        break;
                    case ICue.CueType.Haptic:
                        if (_wayfindingAccessibilitySettings.EnabledHapticCues)
                        {
                            Debug.Log("Haptic cue enabled");
                            cue.Activate(action.Target);
                        } else
                        {
                            cue.Deactivate();
                        }
                        break;
                }
            }
        }
    }
}