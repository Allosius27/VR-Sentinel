using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : AllosiusDev.Singleton<GameCore>
{
    #region Fields

    private List<Cell> listCells = new List<Cell>();

    private PlayerManager playerManager;

    #endregion

    #region Properties

    public List<Cell> ListCells => listCells;

    public PlayerManager PlayerManager => playerManager;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Start()
    {
        Cells cells = FindObjectOfType<Cells>();
        for (int i = 0; i < cells.ListCells.Count; i++)
        {
            listCells.Add(cells.ListCells[i]);
        }
    }
}
