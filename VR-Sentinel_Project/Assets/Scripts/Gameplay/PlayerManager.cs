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

    public Cell cellObjectSelected { get; set; }

    #endregion

    #region UnityInspector

    [SerializeField] private int currentEnergyPoints;

    // a reference to the action
    public SteamVR_Action_Boolean AbsorbObject;
    // a reference to the hand
    public SteamVR_Input_Sources handType;
    

    #endregion

    #region Behaviour

    private void Start()
    {
        playerCanvasManager = FindObjectOfType<PlayerCanvasManager>();

        playerCanvasManager.SetEnergyPoints(currentEnergyPoints);

        AbsorbObject.AddOnStateDownListener(TriggerDown, handType);
        AbsorbObject.AddOnStateUpListener(TriggerUp, handType);
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");
        
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        Absorption();
        
    }

    [ContextMenu("Absorption")]
    public void Absorption()
    {
        Debug.Log("Absorption launched");
        if(cellObjectSelected != null)
        {
            ChangeEnergyPoints(cellObjectSelected.CurrentCellObject.GetComponent<AbsorbableObject>().EnergyPoints);
            cellObjectSelected.SetCellEmpty(true);
            Destroy(cellObjectSelected.CurrentCellObject);
            cellObjectSelected.SetCurrentCellObject(null);
            cellObjectSelected = null;
        }
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
