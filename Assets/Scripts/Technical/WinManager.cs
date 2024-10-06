using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WinManager : MonoBehaviour
{
    public UnityEvent OnWin;

    public void TriggerWin() => OnWin.Invoke();
}
