using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRPointer : MonoBehaviour {

    private int debugTextValue;

    //todo desactiver quand on est opas sur un menu
    [SerializeField] private float defaultLenght = 5f;
    [SerializeField] private GameObject dot;
    [SerializeField] private VRInputModule inputModule;
    //[SerializeField] private Text debugText;

    private LineRenderer lineRenderer;

    private void Awake() {
        lineRenderer ??= GetComponent<LineRenderer>();
    }

    private void Start()
    {
        
    }

    private void Update() {
        if(GameCore.Instance == null)
        {
            return;
        }

        UpdateLine();
    }

    private void UpdateLine() {
        PointerEventData data = inputModule.GetData;
        float targetLenght = data.pointerCurrentRaycast.distance == 0 ? defaultLenght : data.pointerCurrentRaycast.distance;

        RaycastHit hit = CreateRaycast(targetLenght);
        Vector3 endPosition = transform.position + (transform.forward * targetLenght);
        if(hit.collider != null && hit.collider.gameObject.layer != 5) {

            Debug.Log(hit.collider.gameObject.name);

            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell != null)
            {
                if (cell.CanTeleport)
                {
                    cell.SetTeleportPointLocked(false);
                }

                if(cell.CellEmpty == false && cell.CurrentCellObjects.Count >= 1)
                {
                    cell.CurrentCellObjects[cell.CurrentCellObjects.Count-1].GetComponent<AbsorbableObject>().Outline.enabled = true;
                }
                
                GameCore.Instance.PlayerManager.cellObjectSelected = cell;
                if (GameCore.Instance.PlayerManager.constructionModeActive)
                {
                    GameCore.Instance.PlayerManager.PreviewObject();
                }

                cell.SetHoverVisualColor(true);
                
            }

            debugTextValue++;
            if(debugTextValue > 9999)
            {
                debugTextValue = 0;
            }

            /*if (debugText != null)
            {
                debugText.text = debugTextValue.ToString();
            }*/

            endPosition = hit.point;
        }

        dot.transform.position = endPosition;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit CreateRaycast(float lenght) {

        Debug.Log("CreateRaycast");


        for (int i = 0; i < GameCore.Instance.ListCells.Count; i++)
        {
            GameCore.Instance.ListCells[i].SetTeleportPointLocked(true);
            GameCore.Instance.ListCells[i].SetHoverVisualColor(false);
            if(GameCore.Instance.ListCells[i].previewInstantiateObject != null )
            {
                Destroy(GameCore.Instance.ListCells[i].previewInstantiateObject);
            }

            if(GameCore.Instance.ListCells[i].CurrentCellObjects.Count >= 1)
            {
                for (int j = 0; j < GameCore.Instance.ListCells[i].CurrentCellObjects.Count; j++)
                {
                    if (GameCore.Instance.ListCells[i].CurrentCellObjects[j] != null)
                    {
                        GameCore.Instance.ListCells[i].CurrentCellObjects[j].GetComponent<AbsorbableObject>().Outline.enabled = false;
                    }
                }
                
            }
        }

        GameCore.Instance.PlayerManager.cellObjectSelected = null;

        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out RaycastHit hit, defaultLenght);
        return hit;
    }
}
