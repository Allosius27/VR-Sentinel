using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meanie : Sentinel
{
    #region Fields

    private float rotationTurn = 360.0f;
    private float currentTotalRotation = 0.0f;

    #endregion

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void SentinelRotate()
    {
        if (canRotate)
        {
            transform.RotateAround(transform.position, Vector3.up, RotationAngle);

            currentTotalRotation += RotationAngle;
            if(currentTotalRotation >= rotationTurn)
            {
                Debug.Log("Destroy Meanie");

                currentTotalRotation = 0.0f;

            }
        }
    }
}
