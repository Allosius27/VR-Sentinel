using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Cell : MonoBehaviour
{
    #region Fields

    private bool stackable;
    private bool canTeleport;

    private Color baseColor;

    #endregion

    #region Properties

    public bool CellEmpty => cellEmpty;
    public bool Stackable => stackable;
    public bool CanTeleport => canTeleport;

    public Color HoverColor => hoverColor;

    public GameObject previewInstantiateObject { get; set; }

    public List<GameObject> CurrentCellObjects => currentCellObjects;

    public TeleportationPoint TeleportPoint => teleportPoint;
    public Transform ObjectSpawnPoint => objectSpawnPoint;

    #endregion

    #region UnityInspector

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private bool cellEmpty;

    [SerializeField] private Color hoverColor;

    [SerializeField] private TeleportationPoint teleportPoint;
    [SerializeField] private Transform objectSpawnPoint;
    [SerializeField] private List<GameObject> currentCellObjects = new List<GameObject>();

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
        currentCellObjects.Add(_object);
    }

    public void SetCellEmpty(bool value)
    {
        cellEmpty = value;
    }

    public void SetStackableState(bool value)
    {
        stackable = value;
    }

    public void SetCanTeleport(bool value)
    {
        canTeleport = value;
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
