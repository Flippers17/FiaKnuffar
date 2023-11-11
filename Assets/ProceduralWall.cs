using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[SelectionBase]
public class ProceduralWall : MonoBehaviour
{
    [SerializeField]
    private WallSet wallSet;

    public Vector2 _size = Vector2.one;
    [SerializeField]
    private Vector3 _segmentSize = Vector3.one;

    [SerializeField]
    private bool invertedLeft;
    [SerializeField]
    private bool invertedRight;

    public void UpdateWall()
    {
        ClearWall();

        for(int x = 0; x < _size.x; x++)
        {
            for(int y = 0; y < _size.y; y++)
            {
                if(y == _size.y - 1)
                {
                    if (x == 0)
                    {
                        if (!invertedLeft)
                            SpawnWall(wallSet.topCorner, new Vector2(x, y), 0);
                        else
                            SpawnWall(wallSet.topInvertedCorner, new Vector2(x, y), 0);
                    }
                    else if (x == _size.x - 1)
                    {
                        //if (!invertedRight)
                        //    SpawnWall(wallSet.topCorner, new Vector3(x, y, -1), 90);
                        //else
                        //    SpawnWall(wallSet.topInvertedCorner, new Vector3(x, y, -1), -90);
                    }
                    else
                        SpawnWall(wallSet.topWall, new Vector2(x, y), 0);

                }
                else
                {
                    if (x == 0)
                    {
                        if (!invertedLeft)
                            SpawnWall(wallSet.corner, new Vector2(x, y), 0);
                        else
                            SpawnWall(wallSet.invertedCorner, new Vector2(x, y), 0);
                    }
                    else if (x == _size.x - 1)
                    {
                        //if (!invertedRight)
                        //    SpawnWall(wallSet.corner, new Vector3(x, y, -1), 90);
                        //else
                        //    SpawnWall(wallSet.invertedCorner, new Vector3(x, y, -1), -90);
                    }
                    else
                        SpawnWall(wallSet.wall, new Vector2(x, y), 0);
                }
            }
        }
    }

    private void SpawnWall(GameObject wall, Vector3 position, float angle)
    {
        GameObject current = Instantiate(wall, transform);
        current.transform.localPosition = new Vector3(position.x * _segmentSize.x, position.y * _segmentSize.y, position.z * _segmentSize.z);
        current.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    public void ClearWall()
    {
        for(int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            DestroyImmediate(child.gameObject);
        }
    }
}


[CustomEditor(typeof(ProceduralWall))]
public class WallEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Generate wall"))
        {
            ProceduralWall wall = (ProceduralWall)target;

            wall.UpdateWall();
        }
    }
}
