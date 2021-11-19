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

    public PlayerCanvasManager PlayerCanvasManager => playerCanvasManager;
    public GlobalPlayerCanvasManager GlobalPlayerCanvasManager => globalPlayerCanvasManager;

    public CreationSlot currentCreationSlotSelected { get; set; }

    public Cell CurrentPlayerCell { get; set; }
    public Cell cellObjectSelected { get; set; }

    public bool constructionModeActive { get; protected set; }

    public int CurrentEnergyPoints { get; set; }

    #endregion

    #region UnityInspector

    [Header("Sounds :")]

    [SerializeField] private AllosiusDev.AudioData sfxTeleport;
    [SerializeField] private AllosiusDev.AudioData sfxCreateObject;
    [SerializeField] private AllosiusDev.AudioData sfxAbsorbObject;

    [Space]



    [SerializeField] private float timeToActivateSpecialTeleportation;

    //[SerializeField] private float minCellPos = 1.35f;

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
                AllosiusDev.AudioManager.Play(sfxAbsorbObject.sound);

                Sentinel _sentinel = cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count - 1].GetComponent<Sentinel>();
                if (_sentinel != null && _sentinel.GetComponent<Entity>().type == Entity.Type.Sentinel)
                {
                    Debug.Log("Sentinel Absorbed");
                    GameCore.Instance.ListEnemies.Remove(_sentinel.gameObject);
                    canAbsorb = false;
                }
                else if(_sentinel != null && _sentinel.GetComponent<Entity>().type == Entity.Type.Sentrie)
                {
                    GameCore.Instance.ListEnemies.Remove(_sentinel.gameObject);
                }

                ChangeEnergyPoints(cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count - 1].GetComponent<AbsorbableObject>().EnergyPoints, false);

                GameCore.Instance.DestroyCellObject(cellObjectSelected, cellObjectSelected.CurrentCellObjects, cellObjectSelected.CurrentCellObjects.Count - 1);
                cellObjectSelected = null;

                /*if (GameCore.Instance.Sentinel != null)
                {
                    GameCore.Instance.Sentinel.SentinelRotate();
                }*/
                for (int i = 0; i < GameCore.Instance.ListEnemies.Count; i++)
                {
                    GameCore.Instance.ListEnemies[i].GetComponent<Sentinel>().SentinelRotate();
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
            AllosiusDev.AudioManager.Play(sfxTeleport.sound);

            if (GameCore.Instance.Sentinel.PlayerInSightRange)
            {
                GameCore.Instance.InstantiateObject(GameCore.Instance.TreePrefab, CurrentPlayerCell);
            }
            else
            {
                GameCore.Instance.InstantiateObject(GameCore.Instance.SynthoidPrefab, CurrentPlayerCell);
            }

            transform.position = _cellDestination.CurrentCellObjects[_cellDestination.CurrentCellObjects.Count - 1].transform.position;

            CurrentPlayerCell.gameObject.layer = 10;
            for (int i = 0; i < CurrentPlayerCell.transform.childCount; i++)
            {
                if (CurrentPlayerCell.transform.GetChild(i).gameObject != CurrentPlayerCell.VisualDetection)
                {
                    CurrentPlayerCell.transform.GetChild(i).gameObject.layer = 10;
                }
            }
            CurrentPlayerCell.VisualDetection.SetActive(false);

            CurrentPlayerCell = _cellDestination;

            CurrentPlayerCell.gameObject.layer = 0;
            for (int i = 0; i < CurrentPlayerCell.transform.childCount; i++)
            {
                if (CurrentPlayerCell.transform.GetChild(i).gameObject != CurrentPlayerCell.VisualDetection)
                {
                    CurrentPlayerCell.transform.GetChild(i).gameObject.layer = 0;
                }
            }
            CurrentPlayerCell.VisualDetection.SetActive(true);

            GameCore.Instance.DestroyCellObject(_cellDestination, _cellDestination.CurrentCellObjects, _cellDestination.CurrentCellObjects.Count - 1);
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
        if(GameCore.Instance.Sentinel == null && CurrentPlayerCell.isSentinelPiedestal && GameCore.Instance.FinalTeleportationEnergyCost <= CurrentEnergyPoints)
        {
            Debug.Log("Final Teleportation");

            ChangeEnergyPoints(-GameCore.Instance.FinalTeleportationEnergyCost, false);

            Debug.Log("Victory !!!");
            
        }
        else
        {
            Debug.Log("Aleat Teleportation");
            List<Cell> randomCells = new List<Cell>();
            for (int i = 0; i < GameCore.Instance.ListCells.Count; i++)
            {
                if(GameCore.Instance.ListCells[i].transform.position.y <= CurrentPlayerCell.transform.position.y && GameCore.Instance.ListCells[i].CellEmpty 
                    /*&& GameCore.Instance.ListCells[i].transform.position.y >= minCellPos*/)
                {
                    randomCells.Add(GameCore.Instance.ListCells[i]);
                }
            }
            int rnd = Random.Range(0, randomCells.Count);
            Debug.Log(rnd);
            Create(randomCells[rnd], GameCore.Instance.SynthoidPrefab, GameCore.Instance.FinalTeleportationEnergyCost);
            Teleport(randomCells[rnd]);
        }
    }

    [ContextMenu("Create")]
    public void Create(Cell _selectedCell, GameObject _objectToCreate, int _energyCost)
    {
        Debug.Log("Create Object");
        if(_selectedCell != null && currentCreationSlotSelected != null && _energyCost <= CurrentEnergyPoints)
        {
            if(_selectedCell.CellEmpty)
            {
                AllosiusDev.AudioManager.Play(sfxCreateObject.sound);

                GameCore.Instance.InstantiateObject(_objectToCreate, _selectedCell);

                ChangeEnergyPoints(-_energyCost, false);

                /*if (GameCore.Instance.Sentinel != null)
                {
                    GameCore.Instance.Sentinel.SentinelRotate();
                }*/
                for (int i = 0; i < GameCore.Instance.ListEnemies.Count; i++)
                {
                    GameCore.Instance.ListEnemies[i].GetComponent<Sentinel>().SentinelRotate();
                }
            }
            else if(_selectedCell.CellEmpty == false && _selectedCell.Stackable)
            {
                if (_objectToCreate.GetComponent<AbsorbableObject>().PlaceableOnStack)
                {
                    AllosiusDev.AudioManager.Play(sfxCreateObject.sound);

                    GameCore.Instance.InstantiateObject(_objectToCreate, _selectedCell);

                    ChangeEnergyPoints(-_energyCost, false);

                    /*if (GameCore.Instance.Sentinel != null)
                    {
                        GameCore.Instance.Sentinel.SentinelRotate();
                    }*/
                    for (int i = 0; i < GameCore.Instance.ListEnemies.Count; i++)
                    {
                        GameCore.Instance.ListEnemies[i].GetComponent<Sentinel>().SentinelRotate();
                    }
                }
            }
        }
    }

    public void PreviewObject()
    {
        Debug.Log("Preview");
        if (cellObjectSelected != null && currentCreationSlotSelected != null && currentCreationSlotSelected.energyPointsRequired <= CurrentEnergyPoints)
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

    public void ChangeEnergyPoints(int amount, bool canDie)
    {
        if (CurrentEnergyPoints >= 0)
        {
            CurrentEnergyPoints += amount;

            if (CurrentEnergyPoints <= 0 && canDie)
            {
                CurrentEnergyPoints = 0;

                Debug.Log("Game Over");
                GameCore.Instance.GameOver();
            }

            UpdateEnergyPoints();
        }
    }

    public void UpdateEnergyPoints()
    {
        playerCanvasManager.SetEnergyPoints(CurrentEnergyPoints);
    }

    #endregion
}
