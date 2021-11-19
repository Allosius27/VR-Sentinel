using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GlobalPlayerCanvasManager : MonoBehaviour
{
    #region Fields


    #endregion

    #region Properties

    public int currentMenuButtonSelectedIndex { get; set; }
    public int currentEndGameMenuButtonSelectedIndex { get; set; }

    public Image DangerImage => dangerImage;

    public Slider LoadingSlider => loadingSlider;

    public Image FadingImage => fadingImage;

    public GameObject VictoryImage => victoryImage;
    public GameObject GameOverImage => gameOverImage;

    public GameObject Menu => menu;
    public GameObject EndGameMenu => endGameMenu;

    public List<ButtonPause> Buttons => buttons;
    public List<ButtonPause> EndGameButtons => endGameButtons;

    #endregion

    #region UnityInspector

    [SerializeField] private Image dangerImage;

    [SerializeField] private Slider loadingSlider;

    [SerializeField] private Image fadingImage;

    [SerializeField] private GameObject victoryImage;
    [SerializeField] private GameObject gameOverImage;

    [SerializeField] private GameObject menu;
    [SerializeField] private List<ButtonPause> buttons = new List<ButtonPause>();

    [SerializeField] private GameObject endGameMenu;
    [SerializeField] private List<ButtonPause> endGameButtons = new List<ButtonPause>();

    // a reference to the hand
    public SteamVR_Input_Sources handType, handType02;

    // a reference to the action
    public SteamVR_Action_Boolean ChangeButtonSelected;

    // a reference to the action
    public SteamVR_Action_Boolean ActiveButtonSelected;

    #endregion

    #region Behaviour

    private void Start()
    {
        
        

        ChangeButtonSelected.AddOnStateDownListener(ActionTriggerDown, handType02);

        ChangeButtonSelected.AddOnStateDownListener(ActionTriggerDown, handType);

        ActiveButtonSelected.AddOnStateDownListener(ActiveButtonSelectedDown, handType02);

        ActiveButtonSelected.AddOnStateDownListener(ActiveButtonSelectedDown, handType);

        
    }

    public void ActionTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Action Trigger is up");

    }

    public void ActionTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Action Trigger is down");

        if (menu != null && menu.activeInHierarchy)
        {
            currentMenuButtonSelectedIndex++;
            if (currentMenuButtonSelectedIndex > buttons.Count - 1)
            {
                currentMenuButtonSelectedIndex = 0;
            }
            CheckButtonSelected();
        }
        else if(endGameMenu != null && endGameMenu.activeInHierarchy)
        {
            currentEndGameMenuButtonSelectedIndex++;
            if(currentEndGameMenuButtonSelectedIndex > endGameButtons.Count - 1)
            {
                currentEndGameMenuButtonSelectedIndex = 0;
            }
            CheckEndGameButtonSelected();
        }
    }

    public void ActiveButtonSelectedDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("ActiveButtonSelected is down");

        if (menu != null && menu.activeInHierarchy)
        {
            buttons[currentMenuButtonSelectedIndex].Trigger();
        }
        else if(endGameMenu != null && endGameMenu.activeInHierarchy)
        {
            endGameButtons[currentEndGameMenuButtonSelectedIndex].Trigger();
        }
    }

    public void CheckButtonSelected()
    {
        if (menu != null)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i != currentMenuButtonSelectedIndex)
                {
                    buttons[i].GetComponent<Image>().color = buttons[i].notSelectedColor;
                    Debug.Log(buttons[i].GetComponent<Image>().color);
                }
            }
            Debug.Log(buttons[currentMenuButtonSelectedIndex].GetComponent<Image>().color);
            buttons[currentMenuButtonSelectedIndex].GetComponent<Image>().color = buttons[currentMenuButtonSelectedIndex].selectedColor;
        }
    }

    public void CheckEndGameButtonSelected()
    {
        if (endGameMenu != null)
        {
            for (int i = 0; i < endGameButtons.Count; i++)
            {
                    endGameButtons[i].GetComponent<Image>().color = endGameButtons[i].notSelectedColor;
            }
            endGameButtons[currentEndGameMenuButtonSelectedIndex].GetComponent<Image>().color = endGameButtons[currentEndGameMenuButtonSelectedIndex].selectedColor;
        }
    }

    public void OpenMenu()
    {

        if (GameCore.Instance.isPaused)
        {
            Resume();
        }
        else
        {
            GameCore.Instance.isPaused = true;
            menu.SetActive(true);
        }
    }

    public void Resume()
    {
        menu.SetActive(false);
        GameCore.Instance.isPaused = false;
    }

    public void Restart()
    {
        endGameMenu.SetActive(false);
        StartCoroutine(SceneLoader.Instance.LoadAsynchronously(GameCore.Instance.LevelSceneData, 0.2f));
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }

    #endregion
}
