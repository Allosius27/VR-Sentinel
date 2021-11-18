using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalPlayerCanvasManager : MonoBehaviour
{
    #region Properties

    public Image DangerImage => dangerImage;

    public Slider LoadingSlider => loadingSlider;

    public Image FadingImage => fadingImage;

    public GameObject VictoryImage => victoryImage;


    #endregion

    #region UnityInspector

    [SerializeField] private Image dangerImage;

    [SerializeField] private Slider loadingSlider;

    [SerializeField] private Image fadingImage;

    [SerializeField] private GameObject victoryImage;

    #endregion

    #region Behaviour

    private void Start()
    {
        fadingImage.gameObject.SetActive(true);

        victoryImage.SetActive(false);
    }

    #endregion
}
