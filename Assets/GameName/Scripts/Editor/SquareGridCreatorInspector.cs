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

        SquareGridCreator creator = (SquareGridCreator)target;

        if(GUILayout.Button("Create Grid"))
        {
            CreateGrid();
        }

        if(GUILayout.Button("Clear Grid"))
        {
            ClearGrid();
        }

        if(GUILayout.Button("Refresh Grid"))
        {
            RefreshGrid();
        }

        GUILayout.Label(creator.GridArray == null ? "Grid Needs Refreshing" : "Grid Reference is Fine");

        if(GUILayout.Button("Rename Grid"))
        {
            RenameChildren();
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

        creator.ClearGrid();
    }

    void RefreshGrid()
    {
        SquareGridCreator creator = (SquareGridCreator)target;
        //Initalize Grid
        BaseTile[][] TempGridRef = new BaseTile[creator.Width][];

        for (int i = 0; i < TempGridRef.Length; i++)
        {
            TempGridRef[i] = new Tile[creator.Height];
        }

        for (int i = 0; i < creator.transform.childCount; i++)
        {
            Transform tileTrans = creator.transform.GetChild(i);
            BaseTile tempTile = tileTrans.GetComponent<BaseTile>();

            Debug.Log("FoundTile Position: " + tempTile.arrayPosition, tempTile.gameObject);

            TempGridRef[tempTile.arrayPosition.x][tempTile.arrayPosition.y] = tempTile as BaseTile;
        }

        creator.GridArray = TempGridRef;

        Debug.Log("Refreshed Array");
    }

    void RenameChildren()
    {
        SquareGridCreator creator = (SquareGridCreator)target;
        //Initalize Grid
        BaseTile[][] TempGridRef = new BaseTile[creator.Width][];

        for (int i = 0; i < TempGridRef.Length; i++)
        {
            TempGridRef[i] = new Tile[creator.Height];
        }

        for (int i = 0; i < TempGridRef.Length; i++)
        {
            for (int p = 0; p < TempGridRef[i].Length; p++)
            {
                int childNum = (i * creator.Height) + p;

                Transform tileTrans = creator.transform.GetChild(childNum);
                tileTrans.name = "GameSquare(" + i + "," + p + ")" + " num: " + childNum;
            }
        }

        creator.GridArray = TempGridRef;

        Debug.Log("Refreshed Array");
    }
}
