using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerCanvasManager : MonoBehaviour
{
    #region Fields

    private int currentCreationSlotSelectedIndex;

    #endregion

    #region Properties

    public List<CreationSlot> ListCreationsSlots => listCreationsSlots;

    #endregion

    #region UnityInspector

    [SerializeField] private PlayerManager playerManager;

    [SerializeField] private EnergyPoints energyPoints;

    [SerializeField] private List<CreationSlot> listCreationsSlots = new List<CreationSlot>();

    // a reference to the action
    public SteamVR_Action_Boolean ChangeSlotCreateObject;
    // a reference to the hand
    public SteamVR_Input_Sources handType;

    public KeyCode changeSlotKey;

    #endregion

    #region Behaviour

    private void Start()
    {
        ChangeSlotCreateObject.AddOnStateDownListener(TriggerDown, handType);
        ChangeSlotCreateObject.AddOnStateUpListener(TriggerUp, handType);

        UpdateSelectedCreationSlot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(changeSlotKey))
        {
            ChangeSlot();
        }
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");

    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        ChangeSlot();
    }

    [ContextMenu("Change Slot")]
    public void ChangeSlot()
    {
        currentCreationSlotSelectedIndex++;
        if (currentCreationSlotSelectedIndex >= listCreationsSlots.Count)
        {
            currentCreationSlotSelectedIndex = 0;
        }

        UpdateSelectedCreationSlot();
    }

    private void UpdateSelectedCreationSlot()
    {
        for (int i = 0; i < listCreationsSlots.Count; i++)
        {
            if (i == currentCreationSlotSelectedIndex)
            {
                listCreationsSlots[i].SelectedBorder.SetActive(true);
                playerManager.currentCreationSlotSelected = listCreationsSlots[i];
            }
            else
            {
                listCreationsSlots[i].SelectedBorder.SetActive(false);
            }
        }
    }

    public void SetEnergyPoints(int amount)
    {
        energyPoints.SetEnergyPointsAmount(amount);
    }

    #endregion
}
