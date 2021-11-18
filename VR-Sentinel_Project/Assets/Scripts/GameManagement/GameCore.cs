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
    public GameObject MeaniePrefab => meaniePrefab;
    public GameObject TreePrefab => treePrefab;

    public int FinalTeleportationEnergyCost => finalTeleportationEnergyCost;

    #endregion

    #region UnityInspector

    [SerializeField] private AllosiusDev.AudioData mainMusic;

    [SerializeField] private SceneData levelSceneData;

    [SerializeField] private int finalTeleportationEnergyCost = 3;

    [SerializeField] private GameObject synthoidPrefab;
    [SerializeField] private GameObject meaniePrefab;
    [SerializeField] private GameObject treePrefab;

    [SerializeField] private Cell basePlayerCell;

    #endregion

    #region Behaviour

    protected override void Awake()
    {
        base.Awake();

        playerManager = FindObjectOfType<PlayerManager>();

        var sentinels = FindObjectsOfType<Sentinel>();
        for (int i = 0; i < sentinels.Length; i++)
        {
            if (sentinels[i].GetComponent<Entity>().type == Entity.Type.Sentinel)
            {
                sentinel = sentinels[i];
            }
            listEnemies.Add(sentinels[i].gameObject);
        }

        playerManager.CurrentPlayerCell = basePlayerCell;

    }

    private void Start()
    {
        AllosiusDev.AudioManager.Play(mainMusic.sound);

        Cells cells = FindObjectOfType<Cells>();
        for (int i = 0; i < cells.ListCells.Count; i++)
        {
            listCells.Add(cells.ListCells[i]);
        }
    }

    private void Update()
    {
        bool enemyActive = false;
        for (int i = 0; i < ListEnemies.Count; i++)
        {
            if (/*ListEnemies[i] != null && ListEnemies[i].GetComponent<Sentinel>().PlayerInSightRange
                ||*/ ListEnemies[i].GetComponent<Sentinel>().CellPlayerInSightRange)
            {
                enemyActive = true;
            }
        }

        if (enemyActive)
        {
            if(PlayerManager.GlobalPlayerCanvasManager.DangerImage != null)
                PlayerManager.GlobalPlayerCanvasManager.DangerImage.enabled = true;
        }
        else
        {
            if (PlayerManager.GlobalPlayerCanvasManager.DangerImage != null)
                PlayerManager.GlobalPlayerCanvasManager.DangerImage.enabled = false;
        }
    }

    public void DestroyCellObject(Cell cell, List<GameObject> listObjectsCell, int indexObj)
    {
        if (listObjectsCell.Count >= 2)
        {
            cell.SetStackableState(listObjectsCell[indexObj - 1].GetComponent<AbsorbableObject>().StackableObject);
            cell.SetCanTeleport(listObjectsCell[indexObj - 1].GetComponent<AbsorbableObject>().CanTeleportObject);
        }
        Destroy(listObjectsCell[indexObj]);
        listObjectsCell.Remove(listObjectsCell[indexObj]);
        if (listObjectsCell.Count < 1)
        {
            cell.SetCellEmpty(true);
            cell.SetStackableState(false);
            cell.SetCanTeleport(false);
        }
    }

    public void DestroyMeanie(Meanie meanie, Cell cell)
    {
        Debug.Log("Destroy Meanie");

        meanie.SetCurrentTotalRotation(0.0f);
        meanie.sentinelCreator.canRotate = true;
        if (ListEnemies.Contains(this.gameObject))
        {
            ListEnemies.Remove(this.gameObject);
        }
        DestroyCellObject(cell, cell.CurrentCellObjects,
            cell.CurrentCellObjects.Count - 1);
        InstantiateObject(TreePrefab, cell);
    }

    public GameObject InstantiateObject(GameObject objToInstantiate, Cell cell)
    {
        GameObject _object = Instantiate(objToInstantiate);

        cell.AttributeObjectAtCell(_object);

        return _object;
    }

    [ContextMenu("GameOver")]
    public void GameOver()
    {
        StartCoroutine(TimerGameOver());
    }

    private IEnumerator TimerGameOver()
    {
        playerManager.GlobalPlayerCanvasManager.GameOverImage.SetActive(true);

        yield return new WaitForSeconds(2f);

        StartCoroutine(SceneLoader.Instance.LoadAsynchronously(levelSceneData, 0.2f));
    }

    [ContextMenu("Victory")]
    public void Victory()
    {
        StartCoroutine(TimerVictory());
    }

    private IEnumerator TimerVictory()
    {
        playerManager.GlobalPlayerCanvasManager.VictoryImage.SetActive(true);

        yield return new WaitForSeconds(2f);

        playerManager.PlayerCanvasManager.Quit();
    }

    #endregion


}
