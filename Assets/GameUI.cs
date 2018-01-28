using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private PauseScreen m_PauseScreen;
    [SerializeField] private CompleteLevelScreen m_CompleteLevelScreen;

    private GameObject m_CurrentScreen;

    private void Start()
    {
        m_CurrentScreen = null;

        //Set up Buttons
        m_PauseScreen.BackToMenuButton.onClick.AddListener(BackToMainMenu);
    }

    public void ChangeToScreen(GameScreens newScreen)
    {
        if(m_CurrentScreen != null)
        {
            m_CurrentScreen.SetActive(false);
        }

        switch(newScreen)
        {
            case GameScreens.CompleteLevelScreen:
                m_CurrentScreen = m_CompleteLevelScreen.completeLevelObject;
                break;

            case GameScreens.NoScreen:
                m_CurrentScreen = null;
                break;

            case GameScreens.PauseScreen:
                m_CurrentScreen = m_PauseScreen.pauseObject;
                break;
        }

        if(m_CurrentScreen != null)
        {
            m_CurrentScreen.SetActive(false);
        }
    }


    void BackToMainMenu()
    {
        //Clear this UI
        ChangeToScreen(GameScreens.NoScreen);

        GameInstance.instance.ChangeGameState(GameInstance.GameState.MainMenu);
    }

    public enum GameScreens
    {
        PauseScreen,
        CompleteLevelScreen, 
        NoScreen
    }

    [System.Serializable]
	public class PauseScreen
    {
        public GameObject pauseObject;
        public Button ResumeButton, BackToMenuButton;
    }

    [System.Serializable]
    public class CompleteLevelScreen
    {
        public GameObject completeLevelObject;
        public Button BackToMenuButton, NextLevelButton;
    }
}
