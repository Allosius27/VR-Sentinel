using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCore : AllosiusDev.Singleton<GameCore>
{
    #region Fields

    private DebugMenu debugMenu;

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
    [SerializeField] private int baseEnergyPoints = 8;

    #endregion

    #region Behaviour

    protected override void Awake()
    {
        base.Awake();

        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.CurrentEnergyPoints = baseEnergyPoints;

        debugMenu = FindObjectOfType<DebugMenu>();

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

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);

        AllosiusDev.AudioManager.Play(mainMusic.sound);

        Cells cells = FindObjectOfType<Cells>();
        for (int i = 0; i < cells.ListCells.Count; i++)
        {
            listCells.Add(cells.ListCells[i]);
        }

        playerManager.PlayerCanvasManager.SetEnergyPoints(playerManager.CurrentEnergyPoints);

        playerManager.transform.position = playerManager.CurrentPlayerCell.ObjectSpawnPoint.position;

        playerManager.CurrentPlayerCell.gameObject.layer = 0;
        for (int i = 0; i < playerManager.CurrentPlayerCell.transform.childCount; i++)
        {
            if (playerManager.CurrentPlayerCell.transform.GetChild(i).gameObject != playerManager.CurrentPlayerCell.VisualDetection)
            {
                playerManager.CurrentPlayerCell.transform.GetChild(i).gameObject.layer = 0;
            }
        }
        playerManager.CurrentPlayerCell.VisualDetection.SetActive(true);

        playerManager.GlobalPlayerCanvasManager.FadingImage.gameObject.SetActive(true);

        playerManager.GlobalPlayerCanvasManager.VictoryImage.SetActive(false);
        playerManager.GlobalPlayerCanvasManager.GameOverImage.SetActive(false);

        playerManager.GlobalPlayerCanvasManager.currentMenuButtonSelectedIndex = 0;
        playerManager.GlobalPlayerCanvasManager.currentEndGameMenuButtonSelectedIndex = 0;

        playerManager.GlobalPlayerCanvasManager.CheckButtonSelected();
        playerManager.GlobalPlayerCanvasManager.CheckEndGameButtonSelected();

        playerManager.GlobalPlayerCanvasManager.Buttons[playerManager.GlobalPlayerCanvasManager.currentMenuButtonSelectedIndex].GetComponent<Image>().color = 
            playerManager.GlobalPlayerCanvasManager.Buttons[playerManager.GlobalPlayerCanvasManager.currentMenuButtonSelectedIndex].selectedColor;

        playerManager.GlobalPlayerCanvasManager.Buttons[playerManager.GlobalPlayerCanvasManager.currentEndGameMenuButtonSelectedIndex].GetComponent<Image>().color =
            playerManager.GlobalPlayerCanvasManager.Buttons[playerManager.GlobalPlayerCanvasManager.currentEndGameMenuButtonSelectedIndex].selectedColor;

        if (playerManager.GlobalPlayerCanvasManager.Menu != null)
        {
            playerManager.GlobalPlayerCanvasManager.Menu.SetActive(false);
        }

        if (playerManager.GlobalPlayerCanvasManager.EndGameMenu != null)
        {
            playerManager.GlobalPlayerCanvasManager.EndGameMenu.SetActive(false);
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

        yield return new WaitForSeconds(1f);

        debugMenu.OpenEndGameMenu(true);
        /*if (playerManager.GlobalPlayerCanvasManager.EndGameMenu != null)
            playerManager.GlobalPlayerCanvasManager.EndGameMenu.SetActive(true);*/
        //StartCoroutine(SceneLoader.Instance.LoadAsynchronously(levelSceneData, 0.2f));
    }

    [ContextMenu("Victory")]
    public void Victory()
    {
        StartCoroutine(TimerVictory());
    }

    private IEnumerator TimerVictory()
    {
        playerManager.GlobalPlayerCanvasManager.VictoryImage.SetActive(true);

        yield return new WaitForSeconds(1f);

        debugMenu.OpenEndGameMenu(true);
        /*if(playerManager.GlobalPlayerCanvasManager.EndGameMenu != null)
            playerManager.GlobalPlayerCanvasManager.EndGameMenu.SetActive(true);*/
        //playerManager.PlayerCanvasManager.Quit();
    }

    #endregion


}
