using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialsLoaders : MonoBehaviour
{
    [SerializeField] private GameObject audioMan, sceneLoader;

    private void Awake()
    {
        if (AllosiusDev.AudioManager.Instance == null)
        {
            Instantiate(audioMan);
        }

        if (SceneLoader.Instance == null)
        {
            Instantiate(sceneLoader);
        }
    }
}
