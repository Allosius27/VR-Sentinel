using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel : MonoBehaviour
{

    #region Fields


    private bool playerInSightRange;

    private bool cellPlayerInSightRange;

    private float countTimer;

    private bool sentinelAlarmActived;

    #endregion

    #region Properties

    public AllosiusDev.AudioData SfxSentinelDetection => sfxSentinelDetection;
    public AllosiusDev.AudioData AmbientSentinelAbsorption => ambientSentinelAbsorption;

    public bool PlayerInSightRange => playerInSightRange;

    public bool CellPlayerInSightRange => cellPlayerInSightRange;

    public Cell SentinelCell => sentinelCell;

    public SentinelView SentinelView => sentinelView;

    public bool SentinelAlarmActived => sentinelAlarmActived;

    #endregion

    #region UnityInspector

    [SerializeField] private AllosiusDev.AudioData sfxSentinelDetection;
    [SerializeField] private AllosiusDev.AudioData ambientSentinelAbsorption;

    [Space]

    [SerializeField] private SentinelView sentinelView;

    [SerializeField] private float timeInterval;

    [SerializeField] private float rotationAngle = 30.0f;

    [SerializeField] private Cell sentinelCell;

    #endregion

    public virtual void Start()
    {
        sentinelCell.SetCellEmpty(false);
        sentinelCell.SetCurrentCellObject(this.gameObject);
        sentinelCell.isSentinelPiedestal = true;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        playerInSightRange = sentinelView.checkTargetInFieldOfView;
        if(playerInSightRange || CellPlayerInSightRange)
        {
            GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.DangerImage.enabled = true;
            AddCountTime(Time.deltaTime);

            if(!sentinelAlarmActived)
            {
                AllosiusDev.AudioManager.Play(sfxSentinelDetection.sound);

                AllosiusDev.AudioManager.Play(ambientSentinelAbsorption.sound);

                sentinelAlarmActived = true;
            }
        }
        else
        {
            GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.DangerImage.enabled = false;

            if (sentinelAlarmActived)
            {
                AllosiusDev.AudioManager.Stop(ambientSentinelAbsorption.sound);
                sentinelAlarmActived = false;
            }
        }

        cellPlayerInSightRange = sentinelView.checkCellPlayerInFieldOfView;
    }

    public void SetPlayerInSightRange(bool value)
    {
        playerInSightRange = value;
    }

    public void SetSentinelAlarmActived(bool value)
    {
        sentinelAlarmActived = value;
    }


    public void SetCellPlayerInSightRange(bool value)
    {
        cellPlayerInSightRange = value;
    }
    public void AddCountTime(float amount)
    {
        countTimer += amount;

        if (countTimer >= timeInterval)
        {
            countTimer = 0.0f;

            GameCore.Instance.PlayerManager.ChangeEnergyPoints(-1, true);
        }
    }

    [ContextMenu("Sentinel Rotate")]
    public void SentinelRotate()
    {
        transform.RotateAround(transform.position, Vector3.up, rotationAngle);

        //StartCoroutine(CheckCellPlayerInSightRange());
        //StartCoroutine(CheckPlayerInSightRange());
    }

    /*[ContextMenu("CheckPlayerInSightRange")]
    public IEnumerator CheckPlayerInSightRange()
    {
        yield return new WaitForSeconds(0.5f);

        if (playerInSightRange)
        {
            
        }
    }

    [ContextMenu("CheckCellPlayerInSightRange")]
    public IEnumerator CheckCellPlayerInSightRange()
    {
        yield return new WaitForSeconds(0.5f);
    }*/
}
