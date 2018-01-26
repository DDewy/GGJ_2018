using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HexGridCreator))]
public class HexGridCreatorInspector : Editor {

    const float HexSpace = 0.425f;

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
        HexGridCreator creator = (HexGridCreator)target;

        GameObject[][][] tempGrid = new GameObject[(creator.Width * 2) + 1][][];
        
        


    }
}
