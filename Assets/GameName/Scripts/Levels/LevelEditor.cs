using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEditor;

public class LevelEditor : MonoBehaviour {

    public string writeThis = "hi";

    public void getLevel (ref int levelNum, out string levelData)
    {
        //level num is a string so that it can separate and also be more specific - e.g. 001 instead of 1. There is reason to my madness.
        TextAsset levelFile = (TextAsset)Resources.Load("LevelData");
        List<string> allLevels = levelFile.text.Split(',').ToList<string>();
        levelData = allLevels[(levelNum-1)];
    }

    public void storeLevel()
    {
        Debug.Log(Application.dataPath);
        string outputPath = Path.Combine(Application.dataPath, "GameName/Scripts/Levels/");
        Debug.Log(outputPath);
        using (StreamWriter levelFile = new StreamWriter(outputPath + "LevelData.txt", true))
        {
            levelFile.WriteLine(writeThis);
        }
    }
}

//testing
[CustomEditor(typeof(LevelEditor))]
public class LevelEditorEditor : Editor
{
    public int levelToLoad;
    private string loadedLevel;

    public override void OnInspectorGUI()
    {    
        DrawDefaultInspector();
        LevelEditor myLevel = (LevelEditor)target;
        if (GUILayout.Button("Get Level"))
        {
            myLevel.getLevel(ref levelToLoad, out loadedLevel);
        }
        if (GUILayout.Button("Store Level"))
        {
            myLevel.storeLevel();
        }
    }
}
