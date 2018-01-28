using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

public class LevelEditor : MonoBehaviour {

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
        
        List<BaseTile> existingTiles = GameObject.Find("SquareGrid").transform.GetComponentsInChildren<BaseTile>().ToList();
        List<Vector2Int> tileCoords = new List<Vector2Int>();

        //get tile coordinates
        for (int tileNum = 0; tileNum < existingTiles.Count; tileNum++)
        {
            Vector2Int V2Extract;
            ExtractVector2Int(existingTiles[tileNum].name, out V2Extract);
            tileCoords.Insert(tileNum, V2Extract);
        }

        Debug.Log("Remember to clear, create and rename the grid before this");

        //set our custom tiles
        for (int i = 0; i < levelSplit.Count; i++)
        {
            string tileInfo = levelSplit[i];

            //extract tile type
            BaseTile.TileTypes tileType = (BaseTile.TileTypes)System.Enum.Parse(typeof(BaseTile.TileTypes),tileInfo.Substring(0, tileInfo.IndexOf('(')));

            //extract tile coords
            Vector2Int V2Extract;
            ExtractVector2Int(tileInfo, out V2Extract);

            //get current and set tile values
            int findNum = tileCoords.IndexOf(V2Extract);
            BaseTile target = existingTiles[findNum];

            BaseTile.ChangeTo(tileType, target);
            //***********get target return yeehaw

            //set satellite/output values
            if (tileInfo.Contains("+"))
            {
                string extraVal = tileInfo.Substring(tileInfo.IndexOf('+'));
                if (target.tileType == BaseTile.TileTypes.Satalite)
                {
                    ExtractVector2Int(extraVal, out V2Extract);
                    (target as ReflectSatellite).ReflectDirection = V2Extract;
                }
                if (target.tileType == BaseTile.TileTypes.LightOutput)
                {
                    string[] extraVals = extraVal.Split('+');
                    for (int val = 0; val < extraVals.Length; val++)
                    {
                        if (val == 0)
                        {
                            //light direction
                            ExtractVector2Int(extraVals[val], out V2Extract);
                            (target as LightOutput).LightDirection = V2Extract;
                        }
                        else
                        {
                            //target colour
                            Color col;
                            extractColor(extraVals[val], out col);
                            (target as LightOutput).OutputColour = col;
                            Debug.Log("Got Colour: " + col);
                        }
                    }                    
                }
                if (target.tileType == BaseTile.TileTypes.LightTarget)
                {
                    Color col;
                    extractColor(extraVal, out col);
                    (target as TargetTile).TargetColour = col;
                    Debug.Log("Got Colour: " + col);
                }
            }            
        }
    }

    private void ExtractVector2Int(string inString, out Vector2Int output)
    {
        inString = inString.TrimStart('(');
        inString = inString.TrimEnd(')');
        string[] values = inString.Split(',');
        int x = int.Parse(values[0]);
        int y = int.Parse(values[1]);
        output = new Vector2Int(x, y);
    }

    private void extractColor(string inString, out Color output)
    {
        inString = inString.TrimStart('(');
        inString = inString.TrimEnd(')');
        string[] values = inString.Split(',');
        int r = int.Parse(values[0]);
        int g = int.Parse(values[1]);
        int b = int.Parse(values[2]);
        int a = int.Parse(values[3]);
        output = new Vector4(r, g, b, a);
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
