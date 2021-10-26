using UnityEngine.SceneManagement;

public class SceneLoader : AllosiusDev.Singleton<SceneLoader> {
    public event System.Action OnSceneChanged;

    private void Start() {
        ChangeScene(Scenes.MainMenu);
    }

    public void ChangeScene(System.Enum _enum) {
        OnSceneChanged?.Invoke();
        SceneManager.LoadScene((int)(object)_enum);
    }

}
