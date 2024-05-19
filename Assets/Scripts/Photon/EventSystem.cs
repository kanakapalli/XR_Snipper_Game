using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private static EventSystem instance;
    private Dictionary<string, System.Action<object>> eventCallbacks;

    public static EventSystem Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("EventSystem").AddComponent<EventSystem>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        eventCallbacks = new Dictionary<string, System.Action<object>>();
    }

    public void RegisterEvent(string eventName, System.Action<object> callback)
    {
        if (eventCallbacks.ContainsKey(eventName))
        {
            eventCallbacks[eventName] += callback;
        }
        else
        {
            eventCallbacks[eventName] = callback;
        }
    }

    public void UnregisterEvent(string eventName, System.Action<object> callback)
    {
        if (eventCallbacks.ContainsKey(eventName))
        {
            eventCallbacks[eventName] -= callback;
        }
    }

    public void TriggerEvent(string eventName, object data)
    {
        if (eventCallbacks.ContainsKey(eventName))
        {
            eventCallbacks[eventName]?.Invoke(data);
        }
    }
}