using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProcedualBuilding : MonoBehaviour
{
    [SerializeField]
    private List<ProceduralWall> walls;

    [SerializeField]
    private float height = 40f;


    public void GenerateBuilding()
    {
        foreach (ProceduralWall wall in walls)
        {
            wall._size.y = height;
            wall.ClearWall();
            wall.UpdateWall();
        }
    }
}

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
