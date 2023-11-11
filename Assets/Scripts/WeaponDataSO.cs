using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class WeaponDataSO : ScriptableObject
{
    public float pushVelocity;
    public Vector3 areaOfEffect;
    public float greenZoneMultiplier = 1f;
    public float quickTimeSpeedMultiplier = 1f;
}
