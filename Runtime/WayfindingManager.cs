using System.Collections.Generic;
using UnityEngine;

namespace AccessibleWayfinding

{
    public class WayfindingManager : MonoBehaviour
    {
        public static WayfindingManager Instance { get; private set; }
        public IObjective CurrentObjective { get; private set; }
        public IWayfindingAccessibilitySettings WayfindingAccessibilitySettings { get; private set; }

        [SerializeField] public Transform playerTransform;
        
        private readonly List<ICue> _cues = new();
        private bool _cuesChanged;

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

        private void Update(){
            if(_cuesChanged){
                UpdateCuesForCurrentAction(CurrentObjective.CurrentAction);
                _cuesChanged = false;
            }
        }

        public void RegisterCue(ICue cue)
        {
            _cues.Add(cue);
            if (WayfindingAccessibilitySettings == null)
                WayfindingAccessibilitySettings = new MinimalWayfindingAccessibilitySettings
                {
                    EnabledAudioCues = true,
                    EnabledVisualCues = true,
                    EnabledHapticCues = true,
                    EnabledAudioVisualisation = true,
                };
            _cuesChanged = true;
        }
        
        public void UnregisterCue(ICue cue)
        {
            _cues.Remove(cue);
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
            WayfindingAccessibilitySettings = preferences;
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
                Debug.Log(cue.Type);
                switch (cue.Type)
                {
                    case ICue.CueType.Audio:
                        if (WayfindingAccessibilitySettings.EnabledAudioCues)
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
                        if (WayfindingAccessibilitySettings.EnabledVisualCues)
                        {
                            Debug.Log("Visual cue enabled");
                            cue.Activate(action.Target);
                        } else
                        {
                            cue.Deactivate();
                        }
                        break;
                    case ICue.CueType.Haptic:
                        if (WayfindingAccessibilitySettings.EnabledHapticCues)
                        {
                            Debug.Log("Haptic cue enabled");
                            cue.Activate(action.Target);
                        } else
                        {
                            cue.Deactivate();
                        }
                        break;
                    default:
                        cue.Activate(action.Target);
                        break;
                }
            }
        }
    }
}
