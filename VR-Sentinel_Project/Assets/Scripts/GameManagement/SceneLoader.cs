using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : AllosiusDev.Singleton<SceneLoader> {
    public event System.Action OnSceneChanged;

    private void Start() {
        /*if (SceneManager.GetActiveScene().buildIndex == (int)(object)Scenes.BootScene)
        {
            ChangeScene(Scenes.Level);
        }*/
    }

    public void ChangeScene(System.Enum _enum) {
        OnSceneChanged?.Invoke();
        SceneManager.LoadScene((int)(object)_enum);
    }

    public IEnumerator LoadAsynchronously(SceneData _sceneData, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        AllosiusDev.AudioManager.StopAllAmbients();
        Debug.Log("StopAllAmbients");

        GameCore.ResetInstance();

        AsyncOperation operation = SceneManager.LoadSceneAsync((int)(object)_sceneData.sceneToLoad);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //_loadingScreen.slider.value = progress;
            //_loadingScreen.uiProgressBar.SetFill(progress);
            //_loadingScreen.progressText.text = (int)(progress * 100f) + "%";

            if (operation.progress >= 0.8f)
            {
                Debug.Log("SceneChanged");
                AllosiusDev.AudioManager.StopAllMusics();
                //operation.allowSceneActivation = false;
                //yield return new WaitForSeconds(3f);
                //operation.allowSceneActivation = true;
            }

            yield return null;
        }


    }

}
