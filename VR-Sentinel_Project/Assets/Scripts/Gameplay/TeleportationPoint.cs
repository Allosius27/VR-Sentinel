using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationPoint : MonoBehaviour
{
    #region UnityInspector

    [SerializeField] private bool isLocked;

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private Material lockedMat, notLockedMat;

    #endregion

    #region Behaviour

    public void SetLocked(bool value)
    {
        isLocked = value;

        if(isLocked)
        {
            meshRenderer.material = lockedMat;
        }
        else
        {
            meshRenderer.material = notLockedMat;
        }
    }

    #endregion
}
