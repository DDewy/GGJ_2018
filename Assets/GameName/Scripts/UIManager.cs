using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private UIScreens startingUI;

    private UIScreens currentUI;

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

        mainMenu.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);

        switch(startingUI)
        {
            case UIScreens.GameUI:
                gameUI.gameObject.SetActive(true);
                break;

            case UIScreens.MainMenuUI:
                mainMenu.gameObject.SetActive(true);
                break;
        }

        currentUI = startingUI;
    }

    public void SwitchToUI(UIScreens newUI)
    {
        //Deactivate Old
        switch (currentUI)
        {
            case UIScreens.GameUI:
                gameUI.gameObject.SetActive(false);
                break;

            case UIScreens.MainMenuUI:
                mainMenu.gameObject.SetActive(false);
                break;
        }

        currentUI = newUI;

        //Enable the new
        switch (currentUI)
        {
            case UIScreens.GameUI:
                gameUI.gameObject.SetActive(true);
                //Default Game UI Screen
                gameUI.ChangeToScreen(GameUI.GameScreens.GameUIScreen);
                break;

            case UIScreens.MainMenuUI:
                mainMenu.gameObject.SetActive(true);
                //Default UI Screen
                mainMenu.ChangeScreen(MainMenu.Screens.MainMenu);
                break;
        }
    }


    public enum UIScreens
    {
        MainMenuUI,
        GameUI
    }
}
