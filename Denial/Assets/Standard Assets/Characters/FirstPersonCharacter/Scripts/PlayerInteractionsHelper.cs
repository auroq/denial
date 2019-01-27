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

    public void Init()
    {
        Interactions = new ConcurrentDictionary<int, DateTime>();
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
        
    }

    private void HandleLamp(GameObject lamp)
    {
        var light = lamp.GetComponentInChildren<Light>();
        light.enabled = !light.enabled;
    }
}
