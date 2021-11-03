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

    public Cell cellObjectSelected { get; set; }

    #endregion

    #region UnityInspector

    [SerializeField] private Cell currentPlayerCell;

    [SerializeField] private int currentEnergyPoints;

    // a reference to the hand
    public SteamVR_Input_Sources handType, handType02;

    // a reference to the action
    public SteamVR_Action_Boolean AbsorbObject;

    // a reference to the action
    public SteamVR_Action_Boolean CreateObject;

    // a reference to the action
    public SteamVR_Action_Boolean TeleportObject;

    public KeyCode absorptionKey, teleportKey, createObjectKey;
    

    #endregion

    #region Behaviour

    private void Start()
    {
        playerCanvasManager = FindObjectOfType<PlayerCanvasManager>();

        playerCanvasManager.SetEnergyPoints(currentEnergyPoints);

        AbsorbObject.AddOnStateDownListener(AbsorbTriggerDown, handType02);
        AbsorbObject.AddOnStateUpListener(AbsorbTriggerUp, handType02);

        CreateObject.AddOnStateDownListener(CreateTriggerDown, handType);
        CreateObject.AddOnStateUpListener(CreateTriggerUp, handType);

        TeleportObject.AddOnStateDownListener(TeleportTriggerDown, handType);
        TeleportObject.AddOnStateUpListener(TeleportTriggerUp, handType);

        transform.position = currentPlayerCell.ObjectSpawnPoint.position;
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

    }

    public void AbsorbTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Absorption Trigger is up");
        
    }

    public void AbsorbTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Absorption Trigger is down");
        Absorption();
    }

    public void CreateTriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Create Trigger is up");

    }

    public void CreateTriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Create Trigger is down");
        Create();
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

    [ContextMenu("Absorption")]
    public void Absorption()
    {
        Debug.Log("Absorption launched");
        if(cellObjectSelected != null && cellObjectSelected.CurrentCellObjects.Count >= 1)
        {
            ChangeEnergyPoints(cellObjectSelected.CurrentCellObjects[cellObjectSelected.CurrentCellObjects.Count-1].GetComponent<AbsorbableObject>().EnergyPoints);
            
            DestroyCellObject(cellObjectSelected, cellObjectSelected.CurrentCellObjects, cellObjectSelected.CurrentCellObjects.Count - 1);
            cellObjectSelected = null;
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
            currentPlayerCell = cellObjectSelected;
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
            }
            else if(cellObjectSelected.CellEmpty == false && cellObjectSelected.Stackable)
            {
                if (currentCreationSlotSelected.PrefabObjectCreate.GetComponent<AbsorbableObject>().PlaceableOnStack)
                {
                    InstantiateObject(currentCreationSlotSelected.PrefabObjectCreate, cellObjectSelected);

                    ChangeEnergyPoints(-currentCreationSlotSelected.energyPointsRequired);
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
