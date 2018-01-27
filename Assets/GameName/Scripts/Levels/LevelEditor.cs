using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

public class LevelEditor : MonoBehaviour {

    public string writeThis = "hi";
    public int levelToLoad;

    public void getLevel (int levelNum)
    {
        //level num is a string so that it can separate and also be more specific - e.g. 001 instead of 1. There is reason to my madness.
        TextAsset levelFile = (TextAsset)Resources.Load("LevelData");
        List<string> levelSplit = levelFile.text.Split(';').ToList<string>();

        string fullLevelString = levelSplit[(levelNum-1)];
        levelSplit = fullLevelString.Split('|').ToList<string>();


        foreach (string tile in levelSplit)
        {
            BaseTile[][] squareGrid = GameObject.Find("SquareGrid").GetComponent<SquareGridCreator>().GridArray;
            int commaSplit = tile.IndexOf(',');
            int endOfY = tile.LastIndexOf(')');
            int startOfX = tile.IndexOf('(');
            int x = int.Parse(tile.Substring(startOfX, (commaSplit - startOfX)));
            int y = int.Parse(tile.Substring(commaSplit, (endOfY - commaSplit)));
            Debug.Log(tile + " converted to " + x + ", " + y);
        }
        //gridarray from squaregridcreator, set type and call AssignNewTile
        //set reflect dir and sat and lightdir after
    }

    public void storeLevel()
    {
        Debug.Log(Application.dataPath);
        string outputPath = Path.Combine(Application.dataPath, "GameName/Scripts/Levels/");
        Debug.Log(outputPath);

        GameObject squareGrid = GameObject.Find("SquareGrid");
        BaseTile[] allTiles = squareGrid.GetComponentsInChildren<BaseTile>();
        string levelValues = ""; 
        
        foreach (BaseTile tile in allTiles)
        {
            //if (tile.tileType != TileTypes.Tile)
            //{
                //levelValues = levelValues + tile.tileType.ToString() + "|";                
                levelValues = levelValues + (tile as BaseTile).arrayPosition.ToString();   
            //we need ReflectDirection from Satellite and LightDirection from Outputter. Set after Assign.
            //append with '+' in front.
            //}
        }

        Debug.Log(levelValues);

        using (StreamWriter levelFile = new StreamWriter(outputPath + "LevelData.txt", true))
        {
           levelFile.WriteLine(levelValues + ";");
        }
    }
}

//testing
[CustomEditor(typeof(LevelEditor))]
public class LevelEditorEditor : Editor
{    
    public override void OnInspectorGUI()
    {    
        DrawDefaultInspector();
        LevelEditor myLevel = (LevelEditor)target;
        if (GUILayout.Button("Get Level"))
        {
            myLevel.getLevel(myLevel.levelToLoad);
        }
        if (GUILayout.Button("Store Level"))
        {
            myLevel.storeLevel();
        }
    }
}
