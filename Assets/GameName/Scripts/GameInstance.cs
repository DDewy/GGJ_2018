using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour {
    public static GameInstance instance;
    public string GameLevelName;
    public AudioListener MainMenuListener;

    

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

    public enum GameState
    {
        Gameplay,
        MainMenu
    }
}
