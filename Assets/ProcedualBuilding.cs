using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProcedualBuilding : MonoBehaviour
{
    [SerializeField]
    private List<ProceduralWall> walls;
    
    [SerializeField]
    private WallSet wallSet;

    [SerializeField]
    private float height = 40f;


    public void GenerateBuilding()
    {
        foreach (ProceduralWall wall in walls)
        {
            wall._size.y = height;
            wall.wallSet = wallSet;
            wall.ClearWall();
            wall.UpdateWall();
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ProcedualBuilding))]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate Building"))
        {
            ProcedualBuilding building = (ProcedualBuilding)target;
            building.GenerateBuilding();
        }
    }
}
#endif
