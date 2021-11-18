using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelView : FieldOfView
{
	#region Fields

	private Transform absorbableTarget;

	#endregion

	#region Properties

	public Transform AbsorbableTarget => absorbableTarget;

	#endregion

	#region UnityInspector

	[SerializeField] private Animator viewAnimator;

    public LayerMask cellPlayerTargetMask;

	public bool checkCellPlayerInFieldOfView;

	public LayerMask absorbableObjectsTargetMask;

	public bool checkAbsorbableObjectsInFieldOfView;

	#endregion

	#region Behaviour

	public virtual void Update()
    {
		if (viewAnimator != null)
		{
			if (checkCellPlayerInFieldOfView || checkTargetInFieldOfView)
			{
				SetAnimatorSpeed(0.0f);
			}
			else
			{
				SetAnimatorSpeed(1.0f);
			}
		}
    }

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

	protected virtual void FindVisibleAbsorbableObjectsTargets()
	{
		visibleTargets.Clear();
		checkAbsorbableObjectsInFieldOfView = false;
		absorbableTarget = null;
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
					if (target.GetComponent<AbsorbableObject>() != null && target.GetComponent<AbsorbableObject>().cellAssociated != null && target.GetComponent<AbsorbableObject>().EnergyPoints <= 1 &&
						target.GetComponent<Entity>() != null && target.GetComponent<Entity>().type != Entity.Type.Sentinel && target.GetComponent<Entity>().type != Entity.Type.Sentrie
						&& target.GetComponent<Entity>().type != Entity.Type.Meanie)
					{
						absorbableTarget = target;
					}
					else if (target.GetComponent<DetectionAbsorbableObject>() != null && target.GetComponent<DetectionAbsorbableObject>().AbsorbableObject.cellAssociated != null 
						&& target.GetComponent<DetectionAbsorbableObject>().AbsorbableObject.EnergyPoints <= 1)
                    {
						absorbableTarget = target.GetComponent<DetectionAbsorbableObject>().AbsorbableObject.transform;
					}
				}
			}
		}
	}

	protected void SetAnimatorSpeed(float value)
    {
		viewAnimator.speed = value;
    }

	#endregion
}
