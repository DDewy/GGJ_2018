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
        setFilePath();

        //level num is a string so that it can separate and also be more specific - e.g. 001 instead of 1. There is reason to my madness.
        string levelFile = File.ReadAllText(filePath + "LevelData.txt");
        List<string> levelSplit = levelFile.Split(';').ToList<string>();        
        Debug.Log(levelSplit.Count); //last index is blank
        Debug.Log(levelSplit[levelToLoad]);

        string fullLevelString = levelSplit[(levelNum)].TrimStart('|');
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
        for (int i = (levelToLoad == 0) ? 0 : 1; i < levelSplit.Count; i++)
        {
            string tileInfo = levelSplit[i];            
            Debug.Log("TileInfo: " + tileInfo);

            Debug.Log(tileInfo + " is getting tileType");
            //extract tile type
            BaseTile.TileTypes tileType = (BaseTile.TileTypes)System.Enum.Parse(typeof(BaseTile.TileTypes),tileInfo.Substring(0, tileInfo.IndexOf('(')));

            //extract tile coords
            Vector2Int V2Extract;
            ExtractVector2Int(tileInfo, out V2Extract);

            //get current and set tile values
            int findNum = tileCoords.IndexOf(V2Extract);
            BaseTile target = existingTiles[findNum];
            target = BaseTile.ChangeTo(tileType, target);
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
                    //0 is a blank value... much better ways to do this but I am being #lazy
                    bool doneDirection = false;
                    foreach (string val in extraVals)
                    {
                        if (doneDirection == false && val.Length > 1)
                        {
                            //light direction
                            ExtractVector2Int(val, out V2Extract);
                            Debug.Log(extraVal + " returned " + val + " to " + V2Extract);
                            (target as LightOutput).LightDirection = V2Extract;
                        }
                        else if (val.Length > 1)
                        {
                            //target colour
                            Color col;
                            extractColor(val, out col);
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
        if (inString.Length <= 1)
        {
            Debug.Log("inString is " + inString + ", what the fuck.");
            output = new Vector2Int(99, 99);
        }
        else
        {
            inString = inString.Substring(inString.IndexOf('(') + 1, inString.IndexOf(')') - (inString.IndexOf('(') + 1));
            string[] values = inString.Split(',');
            int x, y;
            int.TryParse(values[0], out x);
            int.TryParse(values[1], out y);
            output = new Vector2Int(x, y);
        }
    }

    private void extractColor(string inString, out Color output)
    {
        //inString = inString.Substring(inString.IndexOf('(') + 1, inString.IndexOf(')') - inString.IndexOf('(') + 1);
        //Debug.Log("Trimmed: " + inString);
        string[] values = inString.Split(',');
        int r, g, b, a;
        int.TryParse(values[0], out r);
        int.TryParse(values[1], out g);
        int.TryParse(values[2], out b);
        int.TryParse(values[3], out a);
        output = new Vector4(r, g, b, a);
    }

    public void storeLevel()
    {
        setFilePath();

        GameObject squareGrid = GameObject.Find("SquareGrid");
        BaseTile[] allTiles = squareGrid.GetComponentsInChildren<BaseTile>();
        string levelValues = ""; 
        
        foreach (BaseTile tile in allTiles)
        {
            if (tile.tileType != BaseTile.TileTypes.Tile)
            { 
                levelValues = levelValues + "|" + tile.tileType.ToString() + (tile as BaseTile).arrayPosition.ToString();

                //bonus tile information (append with a + sign)
                if (tile.tileType == BaseTile.TileTypes.Satalite)
                {
                    levelValues = levelValues + "+" + (tile as ReflectSatellite).ReflectDirection.ToString();
                }
                else if (tile.tileType == BaseTile.TileTypes.LightOutput)
                {
                    levelValues = levelValues + "+" + (tile as LightOutput).LightDirection.ToString() + "+" + (tile as LightOutput).OutputColour.ToString();
                }
                else if (tile.tileType == BaseTile.TileTypes.LightTarget)
                {
                    levelValues = levelValues + "+" + (tile as TargetTile).TargetColour.ToString();
                }
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
