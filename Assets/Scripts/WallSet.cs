using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WallSet : ScriptableObject
{
    public GameObject wall;
    public GameObject corner;
    public GameObject invertedCorner;
    public GameObject topWall;
    public GameObject topCorner;
    public GameObject topInvertedCorner;
}
