using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef { get; protected set; }

    public LayerMask playerMask;
    public LayerMask cellsMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer { get; protected set; }
    public bool canSeeCellPlayer { get; protected set; }

    private void Start()
    {
        playerRef = GameCore.Instance.PlayerManager.gameObject;
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            PlayerFieldOfViewCheck();
        }
    }

    private void PlayerFieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;

        Collider[] cellsrangeChecks = Physics.OverlapSphere(transform.position, radius, cellsMask);

        if (cellsrangeChecks.Length != 0)
        {
            Transform target = cellsrangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask) 
                    && target.GetComponent<Cell>() != null && target.GetComponent<Cell>() == GameCore.Instance.PlayerManager.CurrentPlayerCell)
                    canSeeCellPlayer = true;
                else
                    canSeeCellPlayer = false;
            }
            else
                canSeeCellPlayer = false;
        }
        else if (canSeeCellPlayer)
            canSeeCellPlayer = false;
    }
}