using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Cell : MonoBehaviour
{
    #region Fields

    private bool stackable;
    private bool canTeleport;

    private Color baseColor;
    private Material baseMaterial;

    #endregion

    #region Properties

    public bool CellEmpty => cellEmpty;
    public bool Stackable => stackable;
    public bool CanTeleport => canTeleport;

    public bool isSentinelPiedestal { get; set; }

    //public Color HoverColor => hoverColor;
    public Material HoverMaterial => hoverMaterial;

    public GameObject VisualDetection => visualDetection;

    public GameObject previewInstantiateObject { get; set; }

    public List<GameObject> CurrentCellObjects => currentCellObjects;

    public TeleportationPoint TeleportPoint => teleportPoint;
    public Transform ObjectSpawnPoint => objectSpawnPoint;

    #endregion

    #region UnityInspector

    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private bool cellEmpty;

    //[SerializeField] private Color hoverColor;
    [SerializeField] private Material hoverMaterial;

    [SerializeField] private GameObject visualDetection;

    [SerializeField] private TeleportationPoint teleportPoint;
    [SerializeField] private Transform objectSpawnPoint;
    [SerializeField] private List<GameObject> currentCellObjects = new List<GameObject>();

    //[SerializeField] private Transform groundCheck;
    //[SerializeField] private float groundCheckRadius;

    #endregion

    #region Behaviour

    private void Awake()
    {
        baseColor = meshRenderer.material.color;
        baseMaterial = meshRenderer.material;
    }

    private void Start()
    {
        teleportPoint.gameObject.SetActive(true);
        SetTeleportPointLocked(true);

        for (int i = 0; i < currentCellObjects.Count; i++)
        {
            if(currentCellObjects[i].GetComponent<AbsorbableObject>() != null)
            {
                currentCellObjects[i].GetComponent<AbsorbableObject>().cellAssociated = this;
            }
        }

        /*Collider[] hitColliders = Physics.OverlapSphere(transform.position, groundCheckRadius);
        foreach (var hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.name);

            if (hitCollider.gameObject.GetComponent<Cell>() != null && hitCollider.gameObject.transform.position.y > this.transform.position.y)
            {
                Debug.Log(gameObject.name + "not empty");
                SetCellEmpty(false);
            }

            if (hitCollider.gameObject.CompareTag("CollisionCell") && hitCollider.gameObject.transform.position.y >= this.transform.position.y)
            {
                Debug.Log(gameObject.name + "not empty");
                SetCellEmpty(false);
            }
        }*/

        /*if (collision.gameObject.GetComponent<Cell>() != null && collision.gameObject.transform.position.y >= this.transform.position.y)
        {
            Debug.Log(gameObject.name + "not empty");
            SetCellEmpty(false);
        }

        if (collision.gameObject.CompareTag("CollisionCell") && collision.gameObject.transform.position.y >= this.transform.position.y)
        {
            Debug.Log(gameObject.name + "not empty");
            SetCellEmpty(false);
        }*/

    }

    public void SetCurrentCellObject(GameObject _object)
    {
        currentCellObjects.Add(_object);
    }

    public void SetCellEmpty(bool value)
    {
        cellEmpty = value;
    }

    public void SetStackableState(bool value)
    {
        stackable = value;
    }

    public void SetCanTeleport(bool value)
    {
        canTeleport = value;
    }

    public void SetHoverVisualColor(bool value)
    {
        if (value)
        {
            //meshRenderer.material.color = hoverColor;
            meshRenderer.material = hoverMaterial;
        }
        else
        {
            //meshRenderer.material.color = baseColor;
            meshRenderer.material = baseMaterial;
        }
    }

    public void SetTeleportPointLocked(bool value)
    {
        teleportPoint.SetLocked(value);
    }

    #endregion

    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }*/
}
