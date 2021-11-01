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

    #endregion

    #region UnityInspector

    [SerializeField] private int energyPoints;

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
