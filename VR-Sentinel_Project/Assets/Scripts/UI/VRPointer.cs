using UnityEngine;
using UnityEngine.EventSystems;

public class VRPointer : MonoBehaviour {
    //todo desactiver quand on est opas sur un menu
    [SerializeField] private float defaultLenght = 5f;
    [SerializeField] private GameObject dot;
    [SerializeField] private VRInputModule inputModule;

    private LineRenderer lineRenderer;

    private void Awake() {
        lineRenderer ??= GetComponent<LineRenderer>();
    }

    private void Update() {
        UpdateLine();
    }

    private void UpdateLine() {
        PointerEventData data = inputModule.GetData;
        float targetLenght = data.pointerCurrentRaycast.distance == 0 ? defaultLenght : data.pointerCurrentRaycast.distance;

        RaycastHit hit = CreateRaycast(targetLenght);
        Vector3 endPosition = transform.position + (transform.forward * targetLenght);
        if(hit.collider != null) {
            endPosition = hit.point;
        }

        dot.transform.position = endPosition;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRaycast(float lenght) {
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, defaultLenght);
        return hit;
    }
}
