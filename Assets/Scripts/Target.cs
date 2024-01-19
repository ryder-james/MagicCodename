using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField, Range(0, 5)] private int priority = 0;
    
    public int getPriority()
    {
        return priority;
    }

    public void setPriority(int newprio)
    {
        if (newprio >= 0 && newprio <= 5)
        {
            priority = newprio;
        }
        else
        {
            Debug.LogWarning($"Tried to set priority of a {name} to {newprio} when the range is 0-5");
        }
    }
}
