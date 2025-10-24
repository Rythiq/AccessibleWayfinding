using System;
using System.Collections.Generic;
using _Project.Scripts;
using AccessibleWayfinding;
using UnityEngine;

public class AnyOfObjective : SimpleObjective
{
    [SerializeField]
    private string describtion;
    public List<SimpleAction> actions;
    [SerializeField]
    private int actionNeeded;

    private bool _active;
    private int _actionsDone;

    public bool activateOnStart;
    
    public AnyOfObjective nextObjective;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Description = describtion + ":" + _actionsDone + "/" + actionNeeded;
        _active = false;
        CurrentAction = actions[0];
        foreach (var action in actions)
        {
            action.OnActionCompleted += () =>
            {
                actions.Remove(action);
                if(actions.Count>0)
                    CurrentAction = actions[0];
                _actionsDone++;
            };
        }
        if(activateOnStart)
            WayfindingManager.Instance.SetObjective(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(!_active)
            return;
        var playerTransform = WayfindingManager.Instance.playerTransform;
        var oldAction = CurrentAction;
        foreach (var action in actions)
        {
            // if it's closer than current action, make it current action
            if ((playerTransform.position - action.Target.position).magnitude <
                (playerTransform.position - CurrentAction.Target.position).magnitude)
            {
                CurrentAction = action;
            }
        }
        if (oldAction != CurrentAction) Changed();
        if (_actionsDone == actionNeeded)
        {
            IsCompleted = true;
            Changed();
            WayfindingManager.Instance.SetObjective(nextObjective);
            _active = false;
            Destroy(gameObject);
        }
        Description = describtion + ":" + _actionsDone + "/" + actionNeeded;
    }

    public override void Activate()
    {
        _active = true;
    }
}
