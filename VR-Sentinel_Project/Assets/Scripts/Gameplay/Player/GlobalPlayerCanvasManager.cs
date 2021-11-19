using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class GlobalPlayerCanvasManager : MonoBehaviour
{
    #region Fields

    private int currentMenuButtonSelectedIndex;
    private int currentEndGameMenuButtonSelectedIndex;

    #endregion

    #region Properties

    public Image DangerImage => dangerImage;

    public Slider LoadingSlider => loadingSlider;

    public Image FadingImage => fadingImage;

    public GameObject VictoryImage => victoryImage;
    public GameObject GameOverImage => gameOverImage;

    public GameObject Menu => menu;
    public GameObject EndGameMenu => endGameMenu;

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
        fadingImage.gameObject.SetActive(true);

        victoryImage.SetActive(false);
        gameOverImage.SetActive(false);
        

        ChangeButtonSelected.AddOnStateDownListener(ActionTriggerDown, handType02);

        ChangeButtonSelected.AddOnStateDownListener(ActionTriggerDown, handType);

        ActiveButtonSelected.AddOnStateDownListener(ActiveButtonSelectedDown, handType02);

        ActiveButtonSelected.AddOnStateDownListener(ActiveButtonSelectedDown, handType);

        CheckButtonSelected();
        CheckEndGameButtonSelected();
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

        buttons[currentMenuButtonSelectedIndex].Trigger();
    }

    public void CheckButtonSelected()
    {
        if (menu != null)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] == buttons[currentMenuButtonSelectedIndex])
                {
                    buttons[i].GetComponent<Image>().color = buttons[i].selectedColor;
                }
                else
                {
                    buttons[i].GetComponent<Image>().color = buttons[i].notSelectedColor;
                }
            }
        }
    }

    public void CheckEndGameButtonSelected()
    {
        if (endGameMenu != null)
        {
            for (int i = 0; i < endGameButtons.Count; i++)
            {
                if (endGameButtons[i] == endGameButtons[currentEndGameMenuButtonSelectedIndex])
                {
                    endGameButtons[i].GetComponent<Image>().color = endGameButtons[i].selectedColor;
                }
                else
                {
                    endGameButtons[i].GetComponent<Image>().color = endGameButtons[i].notSelectedColor;
                }
            }
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
