using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test_LevelLoader : MonoBehaviour {
    public int levelIndex;
	// Use this for initialization
	void Start ()
    {
        //Grid Creator, Clear, Create, Rename
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects(); //loadedScene.GetRootGameObjects();
        GameObject SquareGridRef = null;
        for (int i = 0; i < rootObjects.Length; i++)
        {
            if (rootObjects[i].name == "SquareGrid")
            {
                SquareGridRef = rootObjects[i];
                break;
            }
        }

        SquareGridCreator creator = SquareGridRef.GetComponent<SquareGridCreator>();

        creator.ClearGrid();
        creator.CreateGrid();
        creator.RenameGrid();

        //Get Level Editor, Get Level of Index
        LevelEditor tempEditor = SquareGridRef.GetComponent<LevelEditor>();
        Debug.Log("Load Level Index: " + levelIndex);
        tempEditor.getLevel(levelIndex);
    }
	
	
}
