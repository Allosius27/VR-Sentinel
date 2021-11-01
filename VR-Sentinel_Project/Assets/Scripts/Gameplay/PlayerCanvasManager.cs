using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanvasManager : MonoBehaviour
{
    #region UnityInspector

    [SerializeField] private EnergyPoints energyPoints;

    #endregion

    #region Behaviour

    public void SetEnergyPoints(int amount)
    {
        energyPoints.SetEnergyPointsAmount(amount);
    }

    #endregion
}
