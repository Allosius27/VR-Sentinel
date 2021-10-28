using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cells : MonoBehaviour
{
    #region Properties

    public List<Cell> ListCells => listCells;

    #endregion

    #region UnityInspector

    [SerializeField] private List<Cell> listCells = new List<Cell>();

    #endregion
}
