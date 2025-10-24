using System;
using AccessibleWayfinding;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ObjectiveOverviewUI : MonoBehaviour, ICue
{
    private TextMeshProUGUI _text;
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Debug.Log("Registering ObjectiveOverviewUI cue");
        WayfindingManager.Instance.RegisterCue(this);
    }

    public ICue.CueType Type => ICue.CueType.Visual;
    public bool IsActive => _text.text != "";
    public void Activate(Transform target)
    {
        Debug.Log("Objective overview activated");
        _text.text = WayfindingManager.Instance.CurrentObjective.Description + "\n \n" +
                    WayfindingManager.Instance.CurrentObjective.CurrentAction.Description;
    }
    public void Deactivate()
    {
        _text.text = "";
    }
}
