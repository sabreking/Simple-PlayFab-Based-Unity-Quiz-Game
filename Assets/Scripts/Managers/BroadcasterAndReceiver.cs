using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BroadcasterAndReceiver : MonoBehaviour
{
    protected EventManager EventSys
    {
        get { return EventManager.Instance; }
    }

    protected virtual void Start()
    {
        SubscribeToEvents();
    }

    protected virtual void SubscribeToEvents() { } 
}
