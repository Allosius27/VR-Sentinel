using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : AllosiusDev.Singleton<GameCore>
{
    #region Fields

    private List<Cell> listCells = new List<Cell>();

    #endregion

    #region Properties

    public List<Cell> ListCells => listCells;

    #endregion

    private void Start()
    {
        Cells cells = FindObjectOfType<Cells>();
        for (int i = 0; i < cells.ListCells.Count; i++)
        {
            listCells.Add(cells.ListCells[i]);
        }
    }
}
