using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meanie : Sentinel
{
    #region Fields

    private float rotationTurn = 360.0f;
    private float currentTotalRotation = 0.0f;

    #endregion

    #region Properties

    public float CurrentTotalRotation => currentTotalRotation;
    public Sentinel sentinelCreator { get; set; }

    #endregion

    public override void Start()
    {
        SentinelCell.SetCellEmpty(false);
        SentinelCell.SetCurrentCellObject(this.gameObject);

        GetComponent<Outline>().enabled = false;

        canRotate = true;
    }

    public override void Update()
    {
        SetPlayerInSightRange(SentinelView.checkTargetInFieldOfView);
        SetCellPlayerInSightRange(SentinelView.checkCellPlayerInFieldOfView);

        if (PlayerInSightRange || CellPlayerInSightRange)
        {

            if(canRotate)
            {
                GameCore.Instance.PlayerManager.SpecialTeleport();
                GameCore.Instance.DestroyMeanie(this, SentinelCell);
            }

            if (!SentinelAlarmActived)
            {
                AllosiusDev.AudioManager.Play(SfxSentinelDetection.sound);

                SetSentinelAlarmActived(true);
            }
        }
        else
        {
            if (SentinelAlarmActived)
            {
                SetSentinelAlarmActived(false);
            }
        }
    }

    public override void SentinelRotate()
    {
        if (canRotate)
        {
            transform.RotateAround(transform.position, Vector3.up, RotationAngle);

            currentTotalRotation += RotationAngle;
            if(currentTotalRotation >= rotationTurn)
            {
                GameCore.Instance.DestroyMeanie(this, SentinelCell);
            }
        }
    }

    public void SetCurrentTotalRotation(float value)
    {
        currentTotalRotation = value;
    }
    
}
