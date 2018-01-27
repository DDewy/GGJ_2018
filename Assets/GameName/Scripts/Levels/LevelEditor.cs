using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

public class LevelEditor : MonoBehaviour {

    //NEED TO ADD/SET TARGET COLOUR
    //NEED TO ADD/SET OUTPUT COLOUR

        //changing getLevel extensively. 
            // square grid: 

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

        //****clear level here somehow
        Debug.Log("Remember to clear/create the grid before this");

        //set our custom tiles
        for (int i = 0; i < levelSplit.Count; i++)
        {
            string tileInfo = levelSplit[i];
            //Debug.Log(tileInfo);                  

            //extract tile type
            Debug.Log("Tile Type is " + tileInfo.Substring(0, tileInfo.IndexOf('(')));
            BaseTile.TileTypes tileType = (BaseTile.TileTypes)System.Enum.Parse(typeof(BaseTile.TileTypes),tileInfo.Substring(0, tileInfo.IndexOf('(')));

            //extract tile coords
            Vector2Int V2Extract;
            ExtractVector2Int(tileInfo, out V2Extract);

            //get current and set tile values
            int findNum = tileCoords.IndexOf(V2Extract);
            BaseTile target = existingTiles[findNum];

            target.ChangeTo(tileType);
            Debug.Log("set to " + target.tileType.ToString());
            target.AssignNewTile(V2Extract);

            Debug.Log(tileInfo + " converted to " + V2Extract.ToString());

            //set satellite/output values
            if (tileInfo.Contains("+"))
            {
                int extraVal = tileInfo.IndexOf('+');
                ExtractVector2Int(tileInfo.Substring(extraVal), out V2Extract);
                Debug.Log("Extracting Vector from: " + tileInfo.Substring(extraVal) + "-- extracted: " + V2Extract);
                if (target.tileType == BaseTile.TileTypes.Satalite)
                {
                    (target as ReflectSatellite).ReflectDirection = V2Extract;
                }
                if (target.tileType == BaseTile.TileTypes.LightOutput)
                {
                    (target as LightOutput).LightDirection = V2Extract;
                    //***extract and set colour
                }
                if (target.tileType == BaseTile.TileTypes.LightTarget)
                {
                    //***extract and set target colour
                }
            }            
        }
    }

    private void ExtractVector2Int(string inString, out Vector2Int output)
    {
        int endOfX = inString.IndexOf(',');
        int endOfY = inString.IndexOf(')');
        int startOfX = inString.IndexOf('(') + 1;
        int startOfY = endOfX + 1;
        //Debug.Log(inString.Substring(startOfX, endOfY - startOfX));
        //Debug.Log("X is " + inString.Substring(startOfX, (endOfX - startOfX)));        
        //Debug.Log("Y is " + inString.Substring(startOfY, (endOfY - startOfY)));        
        int x = int.Parse(inString.Substring(startOfX, (endOfX - startOfX)));
        int y = int.Parse(inString.Substring(startOfY, (endOfY - startOfY)));
        output = new Vector2Int(x, y);
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
