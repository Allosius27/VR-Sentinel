using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HandPosition : MonoBehaviour {
    public Vector3 HandOffset => handOffset;
    [SerializeField] private Vector3 handOffset;

    private void Start() {
        transform.localPosition = handOffset;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(HandPosition))]
internal class HandPositionEditor : Editor {
    public override void OnInspectorGUI() {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI();
        if(EditorGUI.EndChangeCheck()) {
            HandPosition target = this.target as HandPosition;
            target.transform.localPosition = target.HandOffset;
        }


    }

}
#endif
