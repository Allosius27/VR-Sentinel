using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentrie : Sentinel
{
    public override void Start()
    {
        SentinelCell.SetCellEmpty(false);
        SentinelCell.SetCurrentCellObject(this.gameObject);

        canRotate = true;
    }

    public override void Update()
    {
        SetPlayerInSightRange(SentinelView.checkTargetInFieldOfView);
        SetCellPlayerInSightRange(SentinelView.checkCellPlayerInFieldOfView);

        if (PlayerInSightRange ||CellPlayerInSightRange)
        {
            //GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.DangerImage.enabled = true;
            AddCountTime(Time.deltaTime);

            if (!SentinelAlarmActived)
            {
                AllosiusDev.AudioManager.Play(SfxSentinelDetection.sound);

                AllosiusDev.AudioManager.Stop(AmbientSentinelAbsorption.sound);
                AllosiusDev.AudioManager.Play(AmbientSentinelAbsorption.sound);

                SetSentinelAlarmActived(true);
            }
        }
        else
        {
            //GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.DangerImage.enabled = false;

            if (SentinelAlarmActived)
            {
                AllosiusDev.AudioManager.Stop(AmbientSentinelAbsorption.sound);
                SetSentinelAlarmActived(true);
            }
        }

    }
}
