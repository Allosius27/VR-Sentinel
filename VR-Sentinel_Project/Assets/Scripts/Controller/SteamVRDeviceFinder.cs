using System.Collections;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SteamVRDeviceFinder : MonoBehaviour {

    [SerializeField] private SteamVR_Action_Boolean calibrateHandAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("CalibrateRightHand");
    [SerializeField] private bool ignoreHMD = true, ignorebases = true;

    private Coroutine coroutine;
    private Hand hand;
    private SteamVR_TrackedObject trackedObject;

    private void Start() {
        hand = GetComponent<Hand>();
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        if(!trackedObject.isValid) {
            coroutine = StartCoroutine(FindDeviceCoroutine(trackedObject));
        }
    }

    private void FindDevice(SteamVR_TrackedObject obj) {
        if(coroutine != null) {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(FindDeviceCoroutine(obj));
    }

    public IEnumerator FindDeviceCoroutine(SteamVR_TrackedObject obj) {
        obj.index--;
        if(obj.index < SteamVR_TrackedObject.EIndex.Hmd) {
            obj.index = SteamVR_TrackedObject.EIndex.Device16;
        }
        if(ignorebases && (obj.index == SteamVR_TrackedObject.EIndex.Device1 || obj.index == SteamVR_TrackedObject.EIndex.Device2)) {
            obj.index = SteamVR_TrackedObject.EIndex.Hmd;
        }
        if(ignoreHMD && obj.index == SteamVR_TrackedObject.EIndex.Hmd) {
            obj.index = SteamVR_TrackedObject.EIndex.Device16;
        }

        yield return new WaitForEndOfFrame();
        if(!obj.isValid) {
            FindDevice(obj);
        }

    }

    private void Update() {
        if(calibrateHandAction.GetStateDown(hand.handType)) {
            FindDevice(trackedObject);
        }
    }
}
