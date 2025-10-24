using System;
using System.Collections.Generic;
using _Project.Scripts;
using AccessibleWayfinding;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;
    private WayfindingAccessibilitySettings _settings;
    public Gradient defaultGradient;
    public GameObject SettingsPanel;

    public Toggle AudioCues;
    public Toggle AudioVisualisation;
    public Toggle VisualCues;
    public Toggle HapticCues;
    public List<Toggle> GradientToggles;
    public List<Gradient> Gradients;

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
        _settings = new WayfindingAccessibilitySettings
        {
            EnabledAudioCues = true,
            EnabledVisualCues = true,
            EnabledHapticCues = false,
            EnabledAudioVisualisation = false,
            PreferredGradient = defaultGradient
        };
        SettingsPanel.SetActive(false);
    }

    private void UpdateSettings(){
        _settings.EnabledAudioCues = AudioCues.isOn;
        _settings.EnabledAudioVisualisation = AudioVisualisation.isOn;
        _settings.EnabledVisualCues = VisualCues.isOn;
        _settings.EnabledHapticCues = HapticCues.isOn;
        WayfindingManager.Instance.SetPreferences(_settings);
    }
    
    public void OnGradientChanged(int index)
    {
        foreach (Gradient gradient in Gradients)
        {
            if (Gradients.IndexOf(gradient) != index)
            {
                GradientToggles[Gradients.IndexOf(gradient)].SetIsOnWithoutNotify(false);
            }
            else
            {
                GradientToggles[Gradients.IndexOf(gradient)].SetIsOnWithoutNotify(true);
                _settings.PreferredGradient = gradient;
            }
        }
        UpdateSettings();
    }

    public void TogglePause()
    {
        SettingsPanel.SetActive(!SettingsPanel.activeSelf);
    }

    public WayfindingAccessibilitySettings getSettings()
    {
        return _settings;
    }

    public void OnSettingsChanged()
    {
        UpdateSettings();
    }
}
