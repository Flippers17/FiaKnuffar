using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [SerializeField]
    private List<MeshFilter> meshesToBeCombined;
    [SerializeField]
    private MeshFilter meshFilterCombine;

    public void CombineMeshes()
    {
        CombineInstance[] meshes = new CombineInstance[meshesToBeCombined.Count];

        for(int i = 0; i < meshesToBeCombined.Count; ++i)
        {
            meshes[i].mesh = meshesToBeCombined[i].sharedMesh;
        }

        Mesh newMesh = new Mesh();
        newMesh.name = "CombinedBig";
        newMesh.CombineMeshes(meshes, false, false);
        //Unwrapping.GenerateSecondaryUVSet(newMesh);  // This line is necessary!!
        meshFilterCombine.sharedMesh = newMesh;
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(MeshCombiner))]
public class CombineMeshesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Combine"))
        {
            MeshCombiner meshC = (MeshCombiner)target;

            meshC.CombineMeshes();
        }
    }
}

#endif
