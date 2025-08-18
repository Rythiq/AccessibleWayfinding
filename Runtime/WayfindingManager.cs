using UnityEngine;

namespace AccessibleWayfinding

{
    public class WayfindingManager : MonoBehaviour
    {
        public static WayfindingManager Instance { get; private set; }
        public IObjective CurrentObjective { get; private set; }
        public PlayerSettings PlayerSettings { get; private set; }
        
        public void RegisterCue(ICue cue){}
        public void SetObjective(IObjective objective)
        {
            CurrentObjective = objective;
            // Additional logic to handle the new objective can be added here
        }
        public void UpdatePreferences(PlayerSettings preferences)
        {
            PlayerSettings = preferences;
            // Logic to apply the new accessibility preferences can be added here
        }
        public void UpdateCuesForCurrentAction()
        {
            // Logic to update cues based on the current action or objective
        }
    }
}