using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : AllosiusDev.Singleton<GameCore>
{
    #region Fields

    private List<Cell> listCells = new List<Cell>();

    private PlayerManager playerManager;

    private Sentinel sentinel;

    private List<GameObject> listEnemies = new List<GameObject>();

    #endregion

    #region Properties

    public bool isPaused { get; set; }

    public SceneData LevelSceneData => levelSceneData;

    public List<Cell> ListCells => listCells;

    public List<GameObject> ListEnemies => listEnemies;

    public PlayerManager PlayerManager => playerManager;

    public Sentinel Sentinel => sentinel;

    public GameObject SynthoidPrefab => synthoidPrefab;
    public GameObject TreePrefab => treePrefab;

    public int FinalTeleportationEnergyCost => finalTeleportationEnergyCost;

    #endregion

    #region UnityInspector

    [SerializeField] private SceneData levelSceneData;

    [SerializeField] private int finalTeleportationEnergyCost = 3;

    #endregion

    #region Behaviour

    [SerializeField] private GameObject synthoidPrefab;
    [SerializeField] private GameObject treePrefab;

    #endregion

    protected override void Awake()
    {
        base.Awake();

        playerManager = FindObjectOfType<PlayerManager>();

        sentinel = FindObjectOfType<Sentinel>();

        listEnemies.Add(sentinel.gameObject);
    }

    private void Start()
    {
        Cells cells = FindObjectOfType<Cells>();
        for (int i = 0; i < cells.ListCells.Count; i++)
        {
            listCells.Add(cells.ListCells[i]);
        }
    }

    [ContextMenu("GameOver")]
    public void GameOver()
    {
        StartCoroutine(SceneLoader.Instance.LoadAsynchronously(levelSceneData, 0.2f));
    }
}
