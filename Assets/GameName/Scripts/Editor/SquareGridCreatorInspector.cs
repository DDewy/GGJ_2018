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

        if(GUILayout.Button("Clear Grid"))
        {
            ClearGrid();
        }
    }

    void CreateGrid()
    {
        SquareGridCreator creator = (SquareGridCreator)target;
        //Initalize Grid
        GameObject[][] TempGridObject = new GameObject[creator.Width][];
        BaseTile[][] TempGridRef = new BaseTile[creator.Width][];

        for(int i = 0; i < TempGridObject.Length; i++)
        {
            TempGridObject[i] = new GameObject[creator.Height];
            TempGridRef[i] = new Tile[creator.Height];
        }

        //const int SquareSpacing = 1;
        int xOffset = creator.Width / 2, yOffset = creator.Height / 2;
        creator.WorldOffset = new Vector2Int(-xOffset, -yOffset);

        for (int i = 0; i < TempGridObject.Length; i++)
        {
            for(int p = 0; p < TempGridObject[i].Length; p++)
            {
                TempGridObject[i][p] = Instantiate(creator.SquareRef, creator.transform);
                TempGridObject[i][p].transform.localPosition = new Vector3Int(i - xOffset, p - yOffset, 0);
                TempGridRef[i][p] = TempGridObject[i][p].GetComponent<BaseTile>();
                TempGridRef[i][p].AssignNewTile(new Vector2Int(i, p));
            }
        }

        creator.GridArray = TempGridRef;

        Debug.Log("Reached the end of the Creation Array");
    }

    void ClearGrid()
    {
        SquareGridCreator creator = (SquareGridCreator)target;

        BaseTile[][] TempArray = creator.GridArray;

        if(TempArray == null)
        {
            while(creator.transform.childCount > 0)
            {
                DestroyImmediate(creator.transform.GetChild(0).gameObject);
            }
            return;
        }


        for(int i = 0; i < TempArray.Length; i++)
        {
            for(int p = 0; p < TempArray[i].Length; p++)
            {
                DestroyImmediate(TempArray[i][p].gameObject);
                TempArray[i][p] = null;
            }
        }

        creator.GridArray = null;
    }
}
