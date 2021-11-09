using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel : MonoBehaviour
{

    #region Fields


    private bool playerInSightRange;

    private bool cellPlayerInSightRange;

    #endregion

    #region UnityInspector

    [SerializeField] private SentinelView sentinelView;

    #endregion

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = sentinelView.checkTargetInFieldOfView;

        cellPlayerInSightRange = sentinelView.checkCellPlayerInFieldOfView;
    }

    [ContextMenu("Sentinel Rotate")]
    public void SentinelRotate()
    {
        transform.RotateAround(transform.position, Vector3.up, 30.0f);
    }
}
