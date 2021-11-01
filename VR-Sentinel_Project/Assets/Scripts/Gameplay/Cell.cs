using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Cell : MonoBehaviour
{
    #region Fields

    private Color baseColor;

    #endregion

    #region Properties

    public bool CellEmpty => cellEmpty;
    public Color HoverColor => hoverColor;

    public GameObject CurrentCellObject => currentCellObject;

    #endregion

    #region UnityInspector

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private bool cellEmpty;
    [SerializeField] private Color hoverColor;

    [SerializeField] private TeleportPoint teleportPoint;
    [SerializeField] private Transform objectSpawnPoint;
    [SerializeField] private GameObject currentCellObject;

    #endregion

    #region Behaviour

    private void Awake()
    {
        baseColor = meshRenderer.material.color;
    }

    private void Start()
    {
        teleportPoint.gameObject.SetActive(true);
        SetTeleportPointLocked(true);
    }

    public void SetCurrentCellObject(GameObject _object)
    {
        currentCellObject = _object;
    }

    public void SetCellEmpty(bool value)
    {
        cellEmpty = value;
    }

    public void SetHoverVisualColor(bool value)
    {
        if (value)
        {
            meshRenderer.material.color = hoverColor;
        }
        else
        {
            meshRenderer.material.color = baseColor;
        }
    }

    public void SetTeleportPointLocked(bool value)
    {
        teleportPoint.SetLocked(value);
    }

    #endregion
}
