using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Cell : MonoBehaviour
{
    #region UnityInspector

    [SerializeField] private TeleportPoint teleportPoint;

    #endregion

    #region Behaviour

    private void Start()
    {
        teleportPoint.gameObject.SetActive(true);
        SetTeleportPointLocked(true);
    }

    public void SetTeleportPointLocked(bool value)
    {
        teleportPoint.SetLocked(value);
    }

    #endregion
}
