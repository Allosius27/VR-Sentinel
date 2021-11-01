using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPoints : MonoBehaviour
{
    #region UnityInspector

    [SerializeField] private Text energyPointsAmount;

    #endregion

    #region Behaviour

    public void SetEnergyPointsAmount(int amount)
    {
        energyPointsAmount.text = amount.ToString();
    }

    #endregion
}
