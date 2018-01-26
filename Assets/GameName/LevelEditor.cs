using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelEditor : MonoBehaviour {

    public void getLevel (ref int levelNum, out string levelData)
    {
        //level num is a string so that it can separate and also be more specific - e.g. 001 instead of 1. There is reason to my madness.
        TextAsset levelFile = (TextAsset)Resources.Load("LevelData");
        List<string> allLevels = levelFile.text.Split(',').ToList<string>();
        levelData = allLevels[levelNum];
    }

    public void storeLevel()
    {
        //System.IO.File.WriteAllText(Application.dataPath/, text);
        //tear open nox to see how yvan saves paths YEEHAWWW
    }
}
