using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerManager : MonoBehaviour
{
    #region Fields

    private PlayerCanvasManager playerCanvasManager;



    #endregion

    #region Properties

    public CreationSlot currentCreationSlotSelected { get; set; }

    public Cell CurrentPlayerCell => currentPlayerCell;
    public Cell cellObjectSelected { get; set; }

    public bool constructionModeActive { get; protected set; }

    #endregion

    #region UnityInspector

    [SerializeField] private Cell currentPlayerCell;

    [SerializeField] private int currentEnergyPoints;

    // a reference to the hand
    public SteamVR_Input_Sources handType, handType02;

    // a reference to the action
    public SteamVR_Action_Boolean BuildActive;

    // a reference to the action
    public SteamVR_Action_Boolean ActionObject;

    // a reference to the action
    public SteamVR_Action_Boolean TeleportObject;

    public KeyCode absorptionKey, teleportKey, createObjectKey, buildModeActiveKey;
    

    #endregion

    #region Behaviour

    private void Start()
    {
        playerCanvasManager = FindObjectOfType<PlayerCanvasManager>();

        playerCanvasManager.SetEnergyPoints(currentEnergyPoints);

        BuildActive.AddOnStateDownListener(BuildActiveTriggerDown, handType02);
        BuildActive.AddOnStateUpListener(BuildActiveTriggerUp, handType02);

        ActionObject.AddOnStateDownListener(ActionTriggerDown, handType02);
        ActionObject.AddOnStateUpListener(ActionTriggerUp, handType02);

        TeleportObject.AddOnStateDownListener(TeleportTriggerDown, handType);
        TeleportObject.AddOnStateUpListener(TeleportTriggerUp, handType);

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
        if(Input.GetKeyDown(absorptionKey))
        {
            Absorption();
        }
        if (Input.GetKeyDown(teleportKey))
        {
            Teleport();
        }
        if(Input.GetKeyDown(createObjectKey))
        {
            Create();
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
            Create();
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
        Teleport();

    }

    public void BuildActivation(bool value)
    {
        constructionModeActive = value;
    }

    [ContextMenu("Absorption")]
    public void Absorption()
    {
        Debug.Log("Absorption launched");
        if(cellObjectSelected != null && cellObjectSelected.CurrentCellObjects.Count >= 1)
        {
            ChangeEnergyPoints(cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count-1].GetComponent<AbsorbableObject>().EnergyPoints);
            
            DestroyCellObject(cellObjectSelected, cellObjectSelected.CurrentCellObjects, cellObjectSelected.CurrentCellObjects.Count - 1);
            cellObjectSelected = null;

            GameCore.Instance.Sentinel.SentinelRotate();
        }
    }

    [ContextMenu("Teleport")]
    public void Teleport()
    {
        Debug.Log("Teleportation launched");
        if(cellObjectSelected != null && cellObjectSelected.CurrentCellObjects.Count >= 1 && cellObjectSelected.CanTeleport)
        {
            InstantiateObject(GameCore.Instance.SynthoidPrefab, currentPlayerCell);

            transform.position = cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count - 1].transform.position;
            
            currentPlayerCell.gameObject.layer = 10;
            for (int i = 0; i < currentPlayerCell.transform.childCount; i++)
            {
                if (currentPlayerCell.transform.GetChild(i).gameObject != currentPlayerCell.VisualDetection)
                {
                    currentPlayerCell.transform.GetChild(i).gameObject.layer = 10;
                }
            }
            currentPlayerCell.VisualDetection.SetActive(false);

            currentPlayerCell = cellObjectSelected;

            currentPlayerCell.gameObject.layer = 0;
            for (int i = 0; i < currentPlayerCell.transform.childCount; i++)
            {
                if (currentPlayerCell.transform.GetChild(i).gameObject != currentPlayerCell.VisualDetection)
                {
                    currentPlayerCell.transform.GetChild(i).gameObject.layer = 0;
                }
            }
            currentPlayerCell.VisualDetection.SetActive(true);

            DestroyCellObject(cellObjectSelected, cellObjectSelected.CurrentCellObjects, cellObjectSelected.CurrentCellObjects.Count - 1);
            cellObjectSelected = null;

        }
    }

    [ContextMenu("Create")]
    public void Create()
    {
        Debug.Log("Create Object");
        if(cellObjectSelected != null && currentCreationSlotSelected != null && currentCreationSlotSelected.energyPointsRequired <= currentEnergyPoints)
        {
            if(cellObjectSelected.CellEmpty)
            {
                InstantiateObject(currentCreationSlotSelected.PrefabObjectCreate, cellObjectSelected);

                ChangeEnergyPoints(-currentCreationSlotSelected.energyPointsRequired);

                GameCore.Instance.Sentinel.SentinelRotate();
            }
            else if(cellObjectSelected.CellEmpty == false && cellObjectSelected.Stackable)
            {
                if (currentCreationSlotSelected.PrefabObjectCreate.GetComponent<AbsorbableObject>().PlaceableOnStack)
                {
                    InstantiateObject(currentCreationSlotSelected.PrefabObjectCreate, cellObjectSelected);

                    ChangeEnergyPoints(-currentCreationSlotSelected.energyPointsRequired);

                    GameCore.Instance.Sentinel.SentinelRotate();
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

    public void ChangeEnergyPoints(int amount)
    {
        currentEnergyPoints += amount;

        UpdateEnergyPoints();
    }

    public void UpdateEnergyPoints()
    {
        playerCanvasManager.SetEnergyPoints(currentEnergyPoints);
    }

    #endregion
}
