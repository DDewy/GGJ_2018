using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

public class LevelEditor : MonoBehaviour {

    //NEED TO ADD/SET TARGET COLOUR
    //NEED TO ADD/SET OUTPUT COLOUR

    public int levelToLoad;
    public string filePath = "null";

    public void setFilePath()
    {
        filePath = Path.Combine(Application.dataPath, "GameName/Scripts/Levels/");
    }

    public void getLevel (int levelNum)
    {
        if (filePath == "null") setFilePath();

        //level num is a string so that it can separate and also be more specific - e.g. 001 instead of 1. There is reason to my madness.
        string levelFile = File.ReadAllText(filePath + "LevelData.txt");
        List<string> levelSplit = levelFile.Split(';').ToList<string>();

        string fullLevelString = levelSplit[(levelNum)].TrimStart('|');
        //Debug.Log(fullLevelString);
        levelSplit = fullLevelString.Split('|').ToList<string>();

        for (int i = 0; i < levelSplit.Count; i++)
        {
            string tileInfo = levelSplit[i];
            Debug.Log(tileInfo);
            BaseTile[][] squareGrid = GameObject.Find("SquareGrid").GetComponent<SquareGridCreator>().GridArray;
            Debug.Log("Attempting to find GridArray:")
            Debug.Log(GameObject.Find("SquareGrid").GetComponent<SquareGridCreator>().GridArray.ToString());

            //extract tile type
            Debug.Log(tileInfo.Substring(0, tileInfo.IndexOf('(')));
            BaseTile.TileTypes tileType = (BaseTile.TileTypes)System.Enum.Parse(typeof(BaseTile.TileTypes),tileInfo.Substring(0, tileInfo.IndexOf('(')));

            //extract tile coords
            int x, y;
            ExtractVector2Int(tileInfo, out x, out y);

            //set tile values
            Debug.Log("Attempt to access array at " + x + ", " + y);
            BaseTile target = squareGrid[x][y];
            target.tileType = tileType;

            target.AssignNewTile(new Vector2Int(x,y));

            Debug.Log(tileInfo + " converted to " + x + ", " + y);

            //set satellite/output values
            if (tileInfo.Contains("+"))
            {
                int extraVal = tileInfo.IndexOf('+');
                ExtractVector2Int(tileInfo.Substring(extraVal), out x, out y);
                Debug.Log("Extracting Vector from: " + tileInfo.Substring(extraVal) + "-- extracted: " + new Vector2Int(x,y));
                if (target.tileType == BaseTile.TileTypes.Satalite)
                {
                    (target as ReflectSatellite).ReflectDirection = new Vector2Int(x, y);
                }
                if (target.tileType == BaseTile.TileTypes.LightOutput)
                {
                    (target as LightOutput).LightDirection = new Vector2Int(x, y);
                    //***extract and set colour
                }
                if (target.tileType == BaseTile.TileTypes.LightTarget)
                {
                    //***extract and set target colour
                }
            }            
        }
    }

    private void ExtractVector2Int(string inString, out int x, out int y)
    {
        int endOfX = inString.IndexOf(',');
        int endOfY = inString.IndexOf(')');
        int startOfX = inString.IndexOf('(') + 1;
        int startOfY = endOfX + 2;
        //Debug.Log("X is " + inString.Substring(startOfX, (endOfX - startOfX)));
        //Debug.Log("Y is " + inString.Substring(startOfY, (endOfY - startOfY)));
        x = int.Parse(inString.Substring(startOfX, (endOfX - startOfX)));
        y = int.Parse(inString.Substring(startOfY, (endOfY - startOfY)));
    }

    public void storeLevel()
    {
        if (filePath == "null") setFilePath();

        GameObject squareGrid = GameObject.Find("SquareGrid");
        BaseTile[] allTiles = squareGrid.GetComponentsInChildren<BaseTile>();
        string levelValues = ""; 
        
        foreach (BaseTile tile in allTiles)
        {
            if (tile.tileType != BaseTile.TileTypes.Tile)
            {
                levelValues = levelValues + "|" + tile.tileType.ToString() + (tile as BaseTile).arrayPosition.ToString();
                if (tile.tileType == BaseTile.TileTypes.Satalite)
                {
                    levelValues = levelValues + "+" + (tile as ReflectSatellite).ReflectDirection.ToString();
                }
                else if (tile.tileType == BaseTile.TileTypes.LightOutput)
                {
                    levelValues = levelValues + "+" + (tile as LightOutput).LightDirection.ToString();
                }
                //we need ReflectDirection from Satellite and LightDirection from Outputter.Set after Assign.
                //append with '+' in front.
            }
        }

        Debug.Log(levelValues);

        using (StreamWriter levelFile = new StreamWriter(filePath + "LevelData.txt", true))
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
        int levelToLoad = myLevel.levelToLoad;
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
