using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRPointer : MonoBehaviour {

    private int debugTextValue;

    //todo desactiver quand on est opas sur un menu
    [SerializeField] private float defaultLenght = 5f;
    [SerializeField] private GameObject dot;
    [SerializeField] private VRInputModule inputModule;
    [SerializeField] private Text debugText;

    private LineRenderer lineRenderer;

    private void Awake() {
        lineRenderer ??= GetComponent<LineRenderer>();
    }

    private void Start()
    {
        
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

            Debug.Log(hit.collider.gameObject.name);
            debugTextValue++;
            if(debugTextValue > 9999)
            {
                debugTextValue = 0;
            }
            debugText.text = debugTextValue.ToString();
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
