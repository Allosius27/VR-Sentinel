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

    public Transform GroundCheck => groundCheck;
    public float GroundCheckRadius => groundCheckRadius;

    public bool canRotate { get; set; }

    public bool PlayerInSightRange => playerInSightRange;

    public bool CellPlayerInSightRange => cellPlayerInSightRange;

    public Cell SentinelCell => sentinelCell;

    public SentinelView SentinelView => sentinelView;

    public bool SentinelAlarmActived => sentinelAlarmActived;

    public float RotationAngle => rotationAngle;

    #endregion

    #region UnityInspector

    [SerializeField] private AllosiusDev.AudioData sfxSentinelDetection;
    [SerializeField] private AllosiusDev.AudioData ambientSentinelAbsorption;

    [Space]

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private SentinelView sentinelView;

    [SerializeField] private float timeInterval;

    [SerializeField] private float rotationAngle = 30.0f;

    [SerializeField] private Cell sentinelCell;

    #endregion

    public virtual void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.name);

            if (hitCollider.gameObject.GetComponent<Cell>() != null)
            {
                Debug.Log(gameObject.name + "collides");
                SetSentinelCell(hitCollider.gameObject.GetComponent<Cell>());
            }
        }

        sentinelCell.SetCellEmpty(false);
        sentinelCell.SetCurrentCellObject(this.gameObject);
        sentinelCell.isSentinelPiedestal = true;

        GetComponent<Outline>().enabled = false;

        canRotate = true;

    }

    // Update is called once per frame
    public virtual void Update()
    {
        playerInSightRange = sentinelView.checkTargetInFieldOfView;
        cellPlayerInSightRange = sentinelView.checkCellPlayerInFieldOfView;
        if(/*playerInSightRange ||*/ cellPlayerInSightRange)
        {
            //GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.DangerImage.enabled = true;
            AddCountTime(Time.deltaTime);

            if(!sentinelAlarmActived)
            {
                AllosiusDev.AudioManager.Play(sfxSentinelDetection.sound);

                AllosiusDev.AudioManager.Stop(ambientSentinelAbsorption.sound);
                AllosiusDev.AudioManager.Play(ambientSentinelAbsorption.sound);

                sentinelAlarmActived = true;
            }
        }
        else
        {
            //GameCore.Instance.PlayerManager.GlobalPlayerCanvasManager.DangerImage.enabled = false;

            if (sentinelAlarmActived)
            {
                AllosiusDev.AudioManager.Stop(ambientSentinelAbsorption.sound);
                sentinelAlarmActived = false;
            }
        }

        CheckMeanieActivation();

    }

    public void CheckMeanieActivation()
    {
        if (playerInSightRange && !cellPlayerInSightRange)
        {
            if (canRotate && sentinelView.checkAbsorbableObjectsInFieldOfView && sentinelView.AbsorbableTarget != null)
            {
                Cell _cell = sentinelView.AbsorbableTarget.GetComponent<AbsorbableObject>().cellAssociated;
                GameCore.Instance.DestroyCellObject(_cell, sentinelView.AbsorbableTarget.GetComponent<AbsorbableObject>().cellAssociated.CurrentCellObjects,
                    sentinelView.AbsorbableTarget.GetComponent<AbsorbableObject>().cellAssociated.CurrentCellObjects.Count - 1);
                GameObject meanie = GameCore.Instance.InstantiateObject(GameCore.Instance.MeaniePrefab, _cell);
                meanie.GetComponent<Meanie>().sentinelCreator = this;
                meanie.GetComponent<Meanie>().SetSentinelCell(_cell);
                GameCore.Instance.ListEnemies.Add(meanie);

                canRotate = false;
            }
        }
    }

    public void SetPlayerInSightRange(bool value)
    {
        playerInSightRange = value;
    }

    public void SetSentinelAlarmActived(bool value)
    {
        sentinelAlarmActived = value;
    }

    public void SetSentinelCell(Cell cell)
    {
        sentinelCell = cell;
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
    public virtual void SentinelRotate()
    {
        if (canRotate)
        {
            transform.RotateAround(transform.position, Vector3.up, rotationAngle);
        }

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
