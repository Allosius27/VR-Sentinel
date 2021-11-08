using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbableObject : MonoBehaviour
{

    #region Fields

    private Outline outline;

    #endregion

    #region Properties

    public Outline Outline => outline;

    public int EnergyPoints => energyPoints;

    public bool StackableObject => stackableObject;
    public bool CanTeleportObject => canTeleportObject;
    public bool PlaceableOnStack => placeableOnStack;

    public Transform ObjectSpawnPoint => objectSpawnPoint;

    public GameObject PreviewObject => previewObject;

    #endregion

    #region UnityInspector

    [SerializeField] private int energyPoints;

    [SerializeField] private bool stackableObject;
    [SerializeField] private bool canTeleportObject;
    [SerializeField] private bool placeableOnStack;

    [SerializeField] private Transform objectSpawnPoint;

    [SerializeField] private GameObject previewObject;

    #endregion

    #region Behaviour

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void Start()
    {
        //outline.enabled = false;
    }

    #endregion
}
