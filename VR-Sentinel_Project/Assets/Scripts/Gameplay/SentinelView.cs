using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelView : FieldOfView
{
    #region UnityInspector

    public LayerMask cellPlayerTargetMask;

    public bool checkCellPlayerInFieldOfView;

    #endregion

    #region Behaviour

    public override IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
			FindVisiblePlayerCellTargets();

		}
    }

	protected void FindVisiblePlayerCellTargets()
	{
		visibleTargets.Clear();
		checkCellPlayerInFieldOfView = false;
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, cellPlayerTargetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
			{
				float dstToTarget = Vector3.Distance(transform.position, target.position);
				if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					visibleTargets.Add(target);
					checkCellPlayerInFieldOfView = true;
				}
			}
		}
	}

	#endregion
}
