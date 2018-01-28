using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour {
    public static GameInstance instance;
    public string GameLevelName;
    public AudioListener MainMenuListener;

    private int levelIndex;

    private GameState currentState = GameState.MainMenu;

    private void Start()
    {
        if(instance == null || instance == this)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }


    public void ChangeGameState(GameState newState)
    {
        if (currentState == newState)
            return;

        switch (newState)
        {
            case GameState.Gameplay:
                //Make Sure MenuUI is turned off
                UIManager.instance.SwitchToUI(UIManager.UIScreens.GameUI);

                //Load Game Scene
                SceneManager.LoadScene(GameLevelName, LoadSceneMode.Additive);
                

                MainMenuListener.enabled = false;
                break;

            case GameState.MainMenu:
                //Remove Game Scene
                SceneManager.UnloadSceneAsync(GameLevelName);
                //Turn off GameUI and Turn on MenuUI
                UIManager.instance.SwitchToUI(UIManager.UIScreens.MainMenuUI);

                MainMenuListener.enabled = true;
                break;
        }

        currentState = newState;
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.sceneLoaded += GameLevelLoaded;

        this.levelIndex = levelIndex;
    }

    void GameLevelLoaded(Scene loadedScene, LoadSceneMode loadMode)
    {
        if(loadedScene.buildIndex == 1)
        {
            //Load Level if we have Reference to Scene

            //Grid Creator, Clear, Create, Rename
            GameObject[] rootObjects = loadedScene.GetRootGameObjects();
            GameObject SquareGridRef = null;
            for(int i = 0; i < rootObjects.Length; i++)
            {
                if(rootObjects[i].name == "SquareGrid")
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

            SceneManager.sceneLoaded -= GameLevelLoaded;

            creator.LevelComplete += LevelCompleted;
        }

    }

    void LevelCompleted(SquareGridCreator creator)
    {
        creator.LevelComplete -= LevelCompleted;

        UIManager.instance.ShowCompleteLevel();
    }

    public enum GameState
    {
        Gameplay,
        MainMenu
    }
}
