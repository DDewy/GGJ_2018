using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private PauseScreen m_PauseScreen;
    [SerializeField] private CompleteLevelScreen m_CompleteLevelScreen;
    [SerializeField] private GameUIScreen m_GameUI;

    private GameObject m_CurrentScreen;

    private void Start()
    {
        m_CurrentScreen = null;

        //Set up Buttons
        m_PauseScreen.BackToMenuButton.onClick.AddListener(BackToMainMenu);
        m_PauseScreen.ResumeButton.onClick.AddListener(delegate { ChangeToScreen(GameScreens.GameUIScreen); });

        m_GameUI.PauseButton.onClick.AddListener(delegate { ChangeToScreen(GameScreens.PauseScreen); });

        m_CompleteLevelScreen.BackToMenuButton.onClick.AddListener(BackToMainMenu);
        //m_CompleteLevelScreen.NextLevelButton.onClick.AddListener(SomeCodeFunction);

        //Set GameUI as the Starting UI
        m_PauseScreen.pauseObject.SetActive(false);
        m_CompleteLevelScreen.completeLevelObject.SetActive(false);
        m_GameUI.gameUIObject.SetActive(false);

        //Game UI is starting UI
        m_GameUI.gameUIObject.SetActive(true);
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

            case GameScreens.GameUIScreen:
                m_CurrentScreen = m_GameUI.gameUIObject;
                break;
        }

        if(m_CurrentScreen != null)
        {
            m_CurrentScreen.SetActive(true);
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
        GameUIScreen,
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

    [System.Serializable]
    public class GameUIScreen
    {
        public GameObject gameUIObject;
        public Button PauseButton;
    }
}
