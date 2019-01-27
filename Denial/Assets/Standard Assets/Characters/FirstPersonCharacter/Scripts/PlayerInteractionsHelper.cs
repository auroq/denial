using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public enum Clickables
{
    Doorway = 0,
    Lamp = 1,
}

[Serializable]
public class PlayerInteractionsHelper
{
    public float interactionDebounce = .5f;
    
    private ConcurrentDictionary<int, DateTime> Interactions;
    private ConcurrentDictionary<int, bool> Toggles;
    

    public void Init()
    {
        Interactions = new ConcurrentDictionary<int, DateTime>();
        Toggles = new ConcurrentDictionary<int, bool>();
    }

    private void PerformToggleableFunction(int objectId, Action onTrue, Action onFalse)
    {
        var state = Toggles.GetOrAdd(objectId, false);
        
        if (state)
            onTrue.Invoke();
        else
            onFalse.Invoke();
        
        Toggles.AddOrUpdate(objectId, state, (k, v) => !v);
    }

    private bool DebounceComplete(int objectId)
    {
        var now = DateTime.UtcNow;
        if (Interactions.ContainsKey(objectId) &&
            (now - Interactions[objectId]).TotalSeconds < interactionDebounce)
            return false;
        
        return true;
    }

    private void LogDebounce(int objectId)
    {
        var now = DateTime.UtcNow;
        Interactions.AddOrUpdate(objectId, now, (id, date) => now);
    }
    
    public void HandleInteraction(GameObject gameObject)
    {
        var id = gameObject.GetInstanceID();
        if (DebounceComplete(id))
        {
            LogDebounce(id);
            switch (gameObject.tag)
            {
                case nameof(Clickables.Doorway):
                    HandleDoorway(gameObject);
                    break;
                case nameof(Clickables.Lamp):
                    HandleLamp(gameObject);
                    break;
                default:
                    return;
            }
        }
    }

    private void HandleDoorway(GameObject doorway)
    {
        //doorway.transform.Rotate(Vector3.right, 45f, Space.Self);
        PerformToggleableFunction(doorway.GetInstanceID(), 
            () => doorway.transform.Rotate(Vector3.up, 90f, Space.Self),
            () => doorway.transform.Rotate(Vector3.up, -90f, Space.Self)
        );
    }

    private void HandleLamp(GameObject lamp)
    {
        var light = lamp.GetComponentInChildren<Light>();
        light.enabled = !light.enabled;
    }
}
