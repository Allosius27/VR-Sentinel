using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OpenMenu();
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
