using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class HandController : MonoBehaviour {
    [SerializeField] private Hand hand;

    private float palmRadius;
    private Transform palm;

    private SteamVR_Behaviour_Skeleton Skeleton => skeleton ??= GetComponentInChildren<SteamVR_Behaviour_Skeleton>();
    private Transform FingerTip => fingerTip ??= Skeleton.GetBone((int)hand.fingerJointHover);

    private SteamVR_Behaviour_Skeleton skeleton;
    private Transform fingerTip;
    private bool isFinger;
    private BoxCollider boxCollider;

    private void Awake() {
        palm = hand.hoverSphereTransform;
        palmRadius = hand.hoverSphereRadius;
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update() {
        if(isFinger && !boxCollider.enabled) {
            Palm();
        }
    }

    [ContextMenu("Finger")]
    private void Finger() {
        isFinger = true;
        if(FingerTip == null) return;
        hand.hoverSphereTransform = FingerTip;
        hand.hoverSphereRadius = hand.fingerJointHoverRadius;
    }
    [ContextMenu("Palm")]
    private void Palm() {
        isFinger = false;
        hand.hoverSphereTransform = palm;
        hand.hoverSphereRadius = palmRadius;
    }

    private void OnTriggerEnter(Collider other) {
        Finger();
    }
    private void OnTriggerExit(Collider other) {
        Palm();
    }
}
