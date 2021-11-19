using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject endGameMenu;

    private void Start()
    {
        if(GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.Menu != null)
            menu = GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.Menu;

        if (GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.EndGameMenu != null)
            endGameMenu = GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.EndGameMenu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
    }

    public void OpenEndGameMenu(bool value)
    {
        endGameMenu.SetActive(value);
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
        StartCoroutine(SceneLoader.Instance.LoadAsynchronously(GameCore.Instance.LevelSceneData, 0.2f));
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
