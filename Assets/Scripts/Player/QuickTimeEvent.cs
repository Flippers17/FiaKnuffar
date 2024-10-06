using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuickTimeType
{
    timing,
    mashing
}

[System.Serializable]
public class QuickTimeEvent
{
    public QuickTimeType type;
    public float quickTimeSpeed = 50;
    public Vector2Int greenZone = new Vector2Int(40, 60);

}
