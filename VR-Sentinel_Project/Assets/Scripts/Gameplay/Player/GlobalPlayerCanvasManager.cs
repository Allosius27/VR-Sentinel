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


    #endregion

    #region UnityInspector

    [SerializeField] private Image dangerImage;

    [SerializeField] private Slider loadingSlider;

    [SerializeField] private Image fadingImage;

    #endregion
}
