using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Void Event", menuName = "Events/VoidEvent")]
public class VoidEventSO : ScriptableObject
{

    public delegate void VoidEventHandler();
    public event VoidEventHandler Event;


    //Raises the event if there are any handlers attached to it
    public void RaiseEvent()
    {
        if (Event != null)
        {
            Event.Invoke();
        }
    }

    //Checks if an event handler is already registered
    public bool IsHandlerRegistered(VoidEventHandler handler)
    {
        if (Event == null)
        {
            return false;
        }
        foreach (VoidEventHandler registeredHandler in Event.GetInvocationList())
        {
            if (registeredHandler == handler)
            {
                return true;
            }
        }
        return false;
    }

}
