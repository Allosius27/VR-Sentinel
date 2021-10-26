﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: UIElement that responds to VR hands and generates UnityEvents
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Valve.VR.InteractionSystem {
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class UIElement : MonoBehaviour {
        public CustomEvents.UnityEventHand onHandClick;

        protected Hand currentHand;

        //-------------------------------------------------
        public virtual void Awake() {
            Button button = GetComponent<Button>();
            if(button) {
                button.onClick.AddListener(OnButtonClick);
            }
        }


        //-------------------------------------------------
        protected virtual void OnHandHoverBegin(Hand hand) {
            currentHand = hand;
            InputModule.instance.HoverBegin(gameObject);
            ControllerButtonHints.ShowButtonHint(hand, hand.uiInteractAction);
            Debug.Log("OnHandHoverBegin " + gameObject.name);
        }


        //-------------------------------------------------
        protected virtual void OnHandHoverEnd(Hand hand) {
            InputModule.instance.HoverEnd(gameObject);
            ControllerButtonHints.HideButtonHint(hand, hand.uiInteractAction);
            currentHand = null;
            Debug.Log("OnHandHoverEnd " + gameObject.name);
        }


        //-------------------------------------------------
        protected virtual void HandHoverUpdate(Hand hand) {
            if(hand.uiInteractAction != null && hand.uiInteractAction.GetStateDown(hand.handType)) {
                InputModule.instance.Submit(gameObject);
                ControllerButtonHints.HideButtonHint(hand, hand.uiInteractAction);
                Debug.Log("HandHoverUpdate " + gameObject.name);
            }
        }


        //-------------------------------------------------
        protected virtual void OnButtonClick() {
            onHandClick?.Invoke(currentHand);
        }
    }

#if UNITY_EDITOR
    //-------------------------------------------------------------------------
    [UnityEditor.CustomEditor(typeof(UIElement))]
    public class UIElementEditor : UnityEditor.Editor {
        //-------------------------------------------------
        // Custom Inspector GUI allows us to click from within the UI
        //-------------------------------------------------
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            UIElement uiElement = (UIElement)target;
            if(GUILayout.Button("Click")) {
                InputModule.instance.Submit(uiElement.gameObject);
            }
        }
    }
#endif
}