using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SquareGridCreator))]
public class SquareGridCreatorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Create Grid"))
        {
            CreateGrid();
        }
    }

    void CreateGrid()
    {
        SquareGridCreator creator = (SquareGridCreator)target;
        //Initalize Grid
        GameObject[][] TempGrid = new GameObject[creator.Width][];

        for(int i = 0; i < TempGrid.Length; i++)
        {
            TempGrid[i] = new GameObject[creator.Height];
        }

        const float 

        for(int i = 0; i < TempGrid.Length; i++)
        {
            for(int p = 0; p < TempGrid[i].Length; p++)
            {
                TempGrid[i][p] = Instantiate(creator.SquareRef);
                TempGrid[i][p].transform.position = (Vector3) (new Vector2Int(i, p));
            }
        }
    }

}
