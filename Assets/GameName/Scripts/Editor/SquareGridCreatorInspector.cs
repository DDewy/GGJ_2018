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
        if(creator.GridArray == null)
        {
            RefreshGrid();
        }

        if(GUILayout.Button("Rename Grid"))
        {
            RenameChildren();
        }
    }

    void CreateGrid()
    {
        SquareGridCreator creator = (SquareGridCreator)target;

        creator.CreateGrid();
    }

    void ClearGrid()
    {
        SquareGridCreator creator = (SquareGridCreator)target;

        creator.ClearGrid();
    }

    void RefreshGrid()
    {
        SquareGridCreator creator = (SquareGridCreator)target;

        creator.RefreshGrid();
    }

    void RenameChildren()
    {
        SquareGridCreator creator = (SquareGridCreator)target;

        creator.RenameGrid();
    }
}
