using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MainMenuScreen m_MainMenu;
    [SerializeField] private LevelSelectScreen m_LevelSelect;
    [SerializeField] private ConfirmQuitScreen m_ConfirmQuit;

    private GameObject m_CurrentScreen;

    private void Start()
    {
        m_MainMenu.menuOwner = m_LevelSelect.menuOwner = m_ConfirmQuit.menuOwner = this;
        
        //Deactive all Screen
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        //StartingScreen
        m_CurrentScreen = m_MainMenu.mainMenu;
        m_CurrentScreen.SetActive(true);

        //Assign Buttons
        m_MainMenu.StartButton.onClick.AddListener(delegate { ChangeScreen(Screens.LevelSelect); });
        m_MainMenu.QuitButton.onClick.AddListener(delegate { ChangeScreen(Screens.ConfirmQuit); });

        m_LevelSelect.BackButton.onClick.AddListener(delegate { ChangeScreen(Screens.MainMenu); });
        for (int i = 0; i < m_LevelSelect.LevelButtons.Length; i++)
        {
            m_LevelSelect.LevelButtons[i].onClick.AddListener(delegate { m_LevelSelect.OpenLevel(i); });
        }
        
        m_ConfirmQuit.YesButton.onClick.AddListener(delegate { Application.Quit(); });
        m_ConfirmQuit.NoButton.onClick.AddListener(delegate { ChangeScreen(Screens.MainMenu); });
    }

    public void ChangeScreen(Screens changeToScreen)
    {
        m_CurrentScreen.SetActive(false);

        switch(changeToScreen)
        {
            case Screens.ConfirmQuit:
                m_CurrentScreen = m_ConfirmQuit.confirmQuit;
                break;

            case Screens.LevelSelect:
                m_CurrentScreen = m_LevelSelect.levelSelect;
                break;

            case Screens.MainMenu:
                m_CurrentScreen = m_MainMenu.mainMenu;
                break;

            case Screens.NoScreen:
                m_CurrentScreen = null;
                break;
        }

        if(m_CurrentScreen != null)
            m_CurrentScreen.SetActive(true);
    }

    //Main Menu Functions

    public enum Screens
    {
        MainMenu, 
        ConfirmQuit, 
        LevelSelect,
        NoScreen
    }

    [System.Serializable]
    public class MainMenuScreen
    {
        [HideInInspector] public MainMenu menuOwner;
        public GameObject mainMenu;
        public Button StartButton, QuitButton;

        public void Init()
        {
            
        }
    }

    [System.Serializable]
    public class LevelSelectScreen
    {
        [HideInInspector] public MainMenu menuOwner;
        public GameObject levelSelect;
        public Button[] LevelButtons;
        public Button BackButton;
        public GameObject LevelGroupOwner;

        public void OpenLevel(int index)
        {
            menuOwner.ChangeScreen(Screens.NoScreen);

            //Load Level Additionally
            GameInstance.instance.ChangeGameState(GameInstance.GameState.Gameplay);

            //TO DO - Load Level from the index
        }
    }

    [System.Serializable]
    public class ConfirmQuitScreen
    {
        [HideInInspector] public MainMenu menuOwner;
        public GameObject confirmQuit;
        public Button YesButton, NoButton;
    }
}