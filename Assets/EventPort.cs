using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class EventPort : ScriptableObject
{
    public UnityAction OnInvoked;

    public void Invoke()
    {
        OnInvoked?.Invoke();
    }
}
public class EventPort<T> : ScriptableObject
{
    public UnityAction<T> OnInvoked;

    public void Invoke(T par0)
    {
        OnInvoked?.Invoke(par0);
    }
}

public class EventPort<T1, T2> : ScriptableObject
{
    public UnityAction<T1, T2> OnInvoked;

    public void Invoke(T1 par0, T2 par1)
    {
        OnInvoked?.Invoke(par0, par1);
    }
}
