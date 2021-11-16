using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionAbsorbableObject : MonoBehaviour
{
    #region Properties

    public AbsorbableObject AbsorbableObject => absorbableObject;

    #endregion

    #region UnityInspector

    [SerializeField] private AbsorbableObject absorbableObject;

    #endregion
}
