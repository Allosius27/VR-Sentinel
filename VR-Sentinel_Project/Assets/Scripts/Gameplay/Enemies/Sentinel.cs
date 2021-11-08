using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel : MonoBehaviour
{

    #region Fields

    private FieldOfView fieldOfView;

    private bool playerInSightRange;

    #endregion

    private void Awake()
    {
        fieldOfView = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = fieldOfView.canSeePlayer;
    }

    [ContextMenu("Sentinel Rotate")]
    public void SentinelRotate()
    {
        transform.RotateAround(transform.position, Vector3.up, 30.0f);
    }
}
