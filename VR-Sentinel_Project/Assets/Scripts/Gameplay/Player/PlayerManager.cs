using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class PlayerManager : MonoBehaviour
{
    #region Fields

    private PlayerCanvasManager playerCanvasManager;
    private GlobalPlayerCanvasManager globalPlayerCanvasManager;

    private bool canAbsorb = true;

    private bool activeSpecialTeleportation;

    private float specialTeleportTimer;


    #endregion

    #region Properties

    public GlobalPlayerCanvasManager GlobalPlayerCanvasManager => globalPlayerCanvasManager;

    public CreationSlot currentCreationSlotSelected { get; set; }

    public Cell CurrentPlayerCell => currentPlayerCell;
    public Cell cellObjectSelected { get; set; }

    public bool constructionModeActive { get; protected set; }

    #endregion

    #region UnityInspector

    [SerializeField] private Cell currentPlayerCell;

    [SerializeField] private int currentEnergyPoints;

    [SerializeField] private float timeToActivateSpecialTeleportation;

    [SerializeField] private float minCellPos = 1.35f;

    [Space]

    // a reference to the hand
    public SteamVR_Input_Sources handType, handType02;

    // a reference to the action
    public SteamVR_Action_Boolean BuildActive;

    // a reference to the action
    public SteamVR_Action_Boolean ActionObject;

    // a reference to the action
    public SteamVR_Action_Boolean TeleportObject;

    // a reference to the action
    public SteamVR_Action_Boolean SpecialTeleportationLeft;

    // a reference to the action
    public SteamVR_Action_Boolean SpecialTeleportationRight;

    public KeyCode absorptionKey, teleportKey, createObjectKey, buildModeActiveKey, specialTeleportationKey;


    #endregion

    #region Behaviour

    private void Awake()
    {
        
    }

    private void Start()
    {
        playerCanvasManager = FindObjectOfType<PlayerCanvasManager>();
        globalPlayerCanvasManager = FindObjectOfType<GlobalPlayerCanvasManager>();

        playerCanvasManager.SetEnergyPoints(currentEnergyPoints);

        BuildActive.AddOnStateDownListener(BuildActiveTriggerDown, handType02);
        BuildActive.AddOnStateUpListener(BuildActiveTriggerUp, handType02);

        ActionObject.AddOnStateDownListener(ActionTriggerDown, handType02);
        ActionObject.AddOnStateUpListener(ActionTriggerUp, handType02);

        SpecialTeleportationLeft.AddOnStateDownListener(SpecialTeleportationTriggerDown, handType02);
        SpecialTeleportationLeft.AddOnStateUpListener(SpecialTeleportationTriggerUp, handType02);

        globalPlayerCanvasManager.LoadingSlider.maxValue = timeToActivateSpecialTeleportation;
        globalPlayerCanvasManager.LoadingSlider.value = 0;
        globalPlayerCanvasManager.LoadingSlider.gameObject.SetActive(false);

        TeleportObject.AddOnStateDownListener(TeleportTriggerDown, handType);
        TeleportObject.AddOnStateUpListener(TeleportTriggerUp, handType);

        SpecialTeleportationRight.AddOnStateDownListener(SpecialTeleportationTriggerDown, handType);
        SpecialTeleportationRight.AddOnStateUpListener(SpecialTeleportationTriggerUp, handType);

        transform.position = currentPlayerCell.ObjectSpawnPoint.position;

        currentPlayerCell.gameObject.layer = 0;
        for (int i = 0; i < currentPlayerCell.transform.childCount; i++)
        {
            if (currentPlayerCell.transform.GetChild(i).gameObject != currentPlayerCell.VisualDetection)
            {
                currentPlayerCell.transform.GetChild(i).gameObject.layer = 0;
            }
        }
        currentPlayerCell.VisualDetection.SetActive(true);
    }

    private void Update()
    {
        if(GameCore.Instance != null && GameCore.Instance.isPaused)
        {
            return;
        }

        if(activeSpecialTeleportation)
        {
            specialTeleportTimer += Time.deltaTime;
            globalPlayerCanvasManager.LoadingSlider.value = specialTeleportTimer;
            if(specialTeleportTimer >= timeToActivateSpecialTeleportation)
            {
                activeSpecialTeleportation = false;
                specialTeleportTimer = 0;
                globalPlayerCanvasManager.LoadingSlider.gameObject.SetActive(false);
                SpecialTeleport();
            }
        }

        if(Input.GetKeyDown(absorptionKey))
        {
            Absorption();
        }
        if (Input.GetKeyDown(teleportKey))
        {
            Teleport(cellObjectSelected);
        }
        if(Input.GetKeyDown(createObjectKey))
        {
            Create(cellObjectSelected, currentCreationSlotSelected.PrefabObjectCreate, currentCreationSlotSelected.energyPointsRequired);
        }

        if(Input.GetKeyDown(specialTeleportationKey))
        {
            //SpecialTeleport();
            ActiveSpecialTeleportation(true);
        }
        if(Input.GetKeyUp(specialTeleportationKey))
        {
            ActiveSpecialTeleportation(false);
        }

        if(Input.GetKeyDown(buildModeActiveKey) && !constructionModeActive)
        {
            constructionModeActive = true;
        }
        else if (Input.GetKeyDown(buildModeActiveKey) && constructionModeActive)
        {
            constructionModeActive = false;
        }

    }

    public void BuildActiveTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Build Active Trigger is up");
        BuildActivation(false);
    }

    public void BuildActiveTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Build Active Trigger is down");
        BuildActivation(true);
    }

    public void ActionTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Action Trigger is up");

    }

    public void ActionTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Action Trigger is down");

        if (constructionModeActive)
        {
            Create(cellObjectSelected, currentCreationSlotSelected.PrefabObjectCreate, currentCreationSlotSelected.energyPointsRequired);
        }
        else
        {
            Absorption();
        }
    }

    public void TeleportTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Teleport Trigger is up");

    }

    public void TeleportTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Teleport Trigger is down");
        Teleport(cellObjectSelected);

    }

    public void SpecialTeleportationTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Special Teleportation Trigger is up");
        ActiveSpecialTeleportation(false);
    }

    public void SpecialTeleportationTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Special Teleportation Trigger is down");
        ActiveSpecialTeleportation(true);
        //SpecialTeleport();
    }

    public void BuildActivation(bool value)
    {
        constructionModeActive = value;
    }

    [ContextMenu("Absorption")]
    public void Absorption()
    {
        if (canAbsorb)
        {
            Debug.Log("Absorption launched");
            if (cellObjectSelected != null && cellObjectSelected.CurrentCellObjects.Count >= 1)
            {
                Sentinel _sentinel = cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count - 1].GetComponent<Sentinel>();
                if (_sentinel != null)
                {
                    Debug.Log("Sentinel Absorbed");
                    GameCore.Instance.ListEnemies.Remove(_sentinel.gameObject);
                    canAbsorb = false;
                }

                ChangeEnergyPoints(cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count - 1].GetComponent<AbsorbableObject>().EnergyPoints, false);

                DestroyCellObject(cellObjectSelected, cellObjectSelected.CurrentCellObjects, cellObjectSelected.CurrentCellObjects.Count - 1);
                cellObjectSelected = null;

                if (GameCore.Instance.Sentinel != null)
                {
                    GameCore.Instance.Sentinel.SentinelRotate();
                }
            }
        }
    }

    [ContextMenu("Teleport")]
    public void Teleport(Cell _cellDestination)
    {
        Debug.Log("Teleportation launched");
        if(_cellDestination != null && _cellDestination.CurrentCellObjects.Count >= 1 && _cellDestination.CanTeleport)
        {
            if (GameCore.Instance.Sentinel.PlayerInSightRange)
            {
                InstantiateObject(GameCore.Instance.TreePrefab, currentPlayerCell);
            }
            else
            {
                InstantiateObject(GameCore.Instance.SynthoidPrefab, currentPlayerCell);
            }

            transform.position = _cellDestination.CurrentCellObjects[_cellDestination.CurrentCellObjects.Count - 1].transform.position;
            
            currentPlayerCell.gameObject.layer = 10;
            for (int i = 0; i < currentPlayerCell.transform.childCount; i++)
            {
                if (currentPlayerCell.transform.GetChild(i).gameObject != currentPlayerCell.VisualDetection)
                {
                    currentPlayerCell.transform.GetChild(i).gameObject.layer = 10;
                }
            }
            currentPlayerCell.VisualDetection.SetActive(false);

            currentPlayerCell = _cellDestination;

            currentPlayerCell.gameObject.layer = 0;
            for (int i = 0; i < currentPlayerCell.transform.childCount; i++)
            {
                if (currentPlayerCell.transform.GetChild(i).gameObject != currentPlayerCell.VisualDetection)
                {
                    currentPlayerCell.transform.GetChild(i).gameObject.layer = 0;
                }
            }
            currentPlayerCell.VisualDetection.SetActive(true);

            DestroyCellObject(_cellDestination, _cellDestination.CurrentCellObjects, _cellDestination.CurrentCellObjects.Count - 1);
            _cellDestination = null;

        }
    }

    public void ActiveSpecialTeleportation(bool value)
    {
        activeSpecialTeleportation = value;

        if(value)
        {
            specialTeleportTimer = 0;
            globalPlayerCanvasManager.LoadingSlider.gameObject.SetActive(true);
        }
        else
        {
            globalPlayerCanvasManager.LoadingSlider.gameObject.SetActive(false);
        }
    }

    [ContextMenu("Special Teleport")]
    public void SpecialTeleport()
    {
        if(GameCore.Instance.ListEnemies.Count < 1 && currentPlayerCell.isSentinelPiedestal && GameCore.Instance.FinalTeleportationEnergyCost <= currentEnergyPoints)
        {
            Debug.Log("Final Teleportation");

            ChangeEnergyPoints(-GameCore.Instance.FinalTeleportationEnergyCost, false);

            Debug.Log("Victory !!!");
            playerCanvasManager.Quit();
        }
        else
        {
            Debug.Log("Aleat Teleportation");
            List<Cell> randomCells = new List<Cell>();
            for (int i = 0; i < GameCore.Instance.ListCells.Count; i++)
            {
                if(GameCore.Instance.ListCells[i].transform.position.y <= currentPlayerCell.transform.position.y && GameCore.Instance.ListCells[i].CellEmpty 
                    && GameCore.Instance.ListCells[i].transform.position.y >= minCellPos)
                {
                    randomCells.Add(GameCore.Instance.ListCells[i]);
                }
            }

            int rnd = Random.Range(0, randomCells.Count);
            Create(randomCells[rnd], GameCore.Instance.SynthoidPrefab, GameCore.Instance.FinalTeleportationEnergyCost);
            Teleport(randomCells[rnd]);
        }
    }

    [ContextMenu("Create")]
    public void Create(Cell _selectedCell, GameObject _objectToCreate, int _energyCost)
    {
        Debug.Log("Create Object");
        if(_selectedCell != null && currentCreationSlotSelected != null && _energyCost <= currentEnergyPoints)
        {
            if(_selectedCell.CellEmpty)
            {
                InstantiateObject(_objectToCreate, _selectedCell);

                ChangeEnergyPoints(-_energyCost, false);

                if (GameCore.Instance.Sentinel != null)
                {
                    GameCore.Instance.Sentinel.SentinelRotate();
                }
            }
            else if(_selectedCell.CellEmpty == false && _selectedCell.Stackable)
            {
                if (_objectToCreate.GetComponent<AbsorbableObject>().PlaceableOnStack)
                {
                    InstantiateObject(_objectToCreate, _selectedCell);

                    ChangeEnergyPoints(-_energyCost, false);

                    if (GameCore.Instance.Sentinel != null)
                    {
                        GameCore.Instance.Sentinel.SentinelRotate();
                    }
                }
            }
        }
    }

    public void PreviewObject()
    {
        Debug.Log("Preview");
        if (cellObjectSelected != null && currentCreationSlotSelected != null && currentCreationSlotSelected.energyPointsRequired <= currentEnergyPoints)
        {
            if (cellObjectSelected.CellEmpty)
            {
                if(cellObjectSelected.previewInstantiateObject != null)
                {
                    Destroy(cellObjectSelected.previewInstantiateObject);
                }
                GameObject _object = Instantiate(currentCreationSlotSelected.PrefabObjectCreate.GetComponent<AbsorbableObject>().PreviewObject);
                cellObjectSelected.previewInstantiateObject = _object;
                if (cellObjectSelected.CellEmpty)
                {
                    _object.transform.SetParent(cellObjectSelected.ObjectSpawnPoint);
                }
                else
                {
                    _object.transform.SetParent(cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count - 1].GetComponent<AbsorbableObject>().ObjectSpawnPoint);
                }

                _object.transform.localPosition = Vector3.zero;
                _object.transform.rotation = Quaternion.identity;
            }
            else if (cellObjectSelected.CellEmpty == false && cellObjectSelected.Stackable)
            {
                if (currentCreationSlotSelected.PrefabObjectCreate.GetComponent<AbsorbableObject>().PlaceableOnStack)
                {
                    if (cellObjectSelected.previewInstantiateObject != null)
                    {
                        Destroy(cellObjectSelected.previewInstantiateObject);
                    }
                    GameObject _object = Instantiate(currentCreationSlotSelected.PrefabObjectCreate.GetComponent<AbsorbableObject>().PreviewObject);
                    cellObjectSelected.previewInstantiateObject = _object;
                    if (cellObjectSelected.CellEmpty)
                    {

                        _object.transform.SetParent(cellObjectSelected.ObjectSpawnPoint);
                    }
                    else
                    {
                        _object.transform.SetParent(cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count - 1].GetComponent<AbsorbableObject>().ObjectSpawnPoint);
                    }

                    _object.transform.localPosition = Vector3.zero;
                    _object.transform.rotation = Quaternion.identity;
                }
            }
        }
    }

    private void DestroyCellObject(Cell cell, List<GameObject> listObjectsCell, int indexObj)
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

    private void InstantiateObject(GameObject objToInstantiate, Cell cell)
    {
        GameObject _object = Instantiate(objToInstantiate);
        if (cell.CellEmpty)
        {
            cell.SetCellEmpty(false);

            _object.transform.SetParent(cell.ObjectSpawnPoint);
        }
        else
        {
            _object.transform.SetParent(cell.CurrentCellObjects[cell.CurrentCellObjects.Count - 1].GetComponent<AbsorbableObject>().ObjectSpawnPoint);
        }

        _object.transform.localPosition = Vector3.zero;
        _object.transform.rotation = Quaternion.identity;

        AbsorbableObject absorbableObject = _object.GetComponent<AbsorbableObject>();
        cell.SetStackableState(absorbableObject.StackableObject);
        cell.SetCanTeleport(absorbableObject.CanTeleportObject);

        cell.SetCurrentCellObject(_object);
    }

    public void ChangeEnergyPoints(int amount, bool canDie)
    {
        if (currentEnergyPoints >= 0)
        {
            currentEnergyPoints += amount;

            if (currentEnergyPoints <= 0 && canDie)
            {
                currentEnergyPoints = 0;

                Debug.Log("Game Over");
                GameCore.Instance.GameOver();
            }

            UpdateEnergyPoints();
        }
    }

    public void UpdateEnergyPoints()
    {
        playerCanvasManager.SetEnergyPoints(currentEnergyPoints);
    }

    #endregion
}
