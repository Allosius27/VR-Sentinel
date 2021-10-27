using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Test : MonoBehaviour
{
    // a reference to the action
    public SteamVR_Action_Boolean SphereOnOff;
    // a reference to the hand
    public SteamVR_Input_Sources handType;
    //reference to the sphere
    public GameObject Sphere;
    public Color newColor;
    public Color baseColor;

    void Start()
    {
        SphereOnOff.AddOnStateDownListener(TriggerDown, handType);
        SphereOnOff.AddOnStateUpListener(TriggerUp, handType);
    }
    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is up");
        Sphere.GetComponent<MeshRenderer>().material.color = newColor;
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Trigger is down");
        Sphere.GetComponent<MeshRenderer>().material.color = baseColor;
    }

    void Update()
    {

    }
}
