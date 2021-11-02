using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationSlot : MonoBehaviour
{
    #region Properties

    public int energyPointsRequired { get; protected set; }

    public GameObject SelectedBorder => selectedBorder;

    public GameObject PrefabObjectCreate => prefabObjectCreate;

    #endregion

    #region UnityInspector

    [SerializeField] private GameObject selectedBorder;
    [SerializeField] private GameObject prefabObjectCreate;

    #endregion

    #region Behaviour

    private void Awake()
    {
        energyPointsRequired = prefabObjectCreate.GetComponent<AbsorbableObject>().EnergyPoints;
    }

    #endregion
}
