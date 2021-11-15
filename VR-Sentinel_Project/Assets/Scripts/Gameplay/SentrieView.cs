using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentrieView : SentinelView
{
    #region UnityInspector

    

    #endregion

    #region Behaviour

    public override IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            FindVisiblePlayerCellTargets();
			FindVisibleAbsorbableObjectsTargets();
        }
    }

	protected override void FindVisibleAbsorbableObjectsTargets()
	{
		visibleTargets.Clear();
		checkAbsorbableObjectsInFieldOfView = false;
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, absorbableObjectsTargetMask);

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
					checkAbsorbableObjectsInFieldOfView = true;
					if(target.GetComponent<AbsorbableObject>() != null && target.GetComponent<AbsorbableObject>().cellAssociated != null && target.GetComponent<AbsorbableObject>().EnergyPoints > 1 &&
						target.GetComponent<Entity>() != null && target.GetComponent<Entity>().type != Entity.Type.Sentinel && target.GetComponent<Entity>().type != Entity.Type.Sentrie
						&& target.GetComponent<Entity>().type != Entity.Type.Meanie)
                    {
						GameCore.Instance.DestroyCellObject(target.GetComponent<AbsorbableObject>().cellAssociated,
							target.GetComponent<AbsorbableObject>().cellAssociated.CurrentCellObjects, target.GetComponent<AbsorbableObject>().cellAssociated.CurrentCellObjects.Count - 1);
                    }
				}
			}
		}
	}

	#endregion
}
