using System;
using UnityEngine;

/// <summary>
/// Mode to place, rotate, and delete furniture objects.
/// </summary>
public class FurnitureMode : Mode
{
    // inspector fields
    [SerializeField]
    private GameObject placeableObjPrefab = null;
    [SerializeField]
    private GameObject furniturePanel = null;

    // variables and references
    private Camera cam = null;
    private GameObject currPlaceableObj = null;
    private BuildObject highlightedObj = null;
    private float mouseWheelRotation;
    private bool isOverUI = false;
    private bool isDestroying = false;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive)
        {
            if (isDestroying)
            {
                if (Input.GetMouseButtonDown(0)) DeleteObject();
                else HighlightObject();
            }
            else
            {
                if (currPlaceableObj == null) currPlaceableObj = Instantiate(placeableObjPrefab);

                if (currPlaceableObj != null)
                {
                    MoveCurrPlaceableObjToMouse();
                    RotateFromMouseWheel();
                    ReleaseIfClicked();
                }

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    Destroy(currPlaceableObj);
                    currPlaceableObj = null;
                    isDestroying = true;
                }
            }
        }
    }

    /// <summary>
    /// Highlights the object being pointed at.
    /// </summary>
    private void HighlightObject()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000f, LayerMask.GetMask("Object")))
        {
            if (highlightedObj == null || highlightedObj != hitInfo.collider.GetComponent<BuildObject>())
            {
                if (highlightedObj) highlightedObj.Dehighlight();
                highlightedObj = hitInfo.collider.GetComponent<BuildObject>();
            }

            if (!highlightedObj.IsHighlighted) highlightedObj.Highlight();
        }
    }

    /// <summary>
    /// Deletes the object being pointed at.
    /// </summary>
    private void DeleteObject()
    {
        if (highlightedObj)
        {
            Destroy(highlightedObj.gameObject);
            highlightedObj = null;
        }
    }

    /// <summary>
    /// Places the furniture object when clicked.
    /// </summary>
    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0) && !isOverUI)
        {
            Collider[] hitColliders = Physics.OverlapSphere(currPlaceableObj.transform.localPosition, 0.2f, LayerMask.GetMask("Wall", "Door Wall", "Object"));

            if (hitColliders.Length > 1)
            {
                Debug.LogWarning("Cannot place there: There is already another object.");
                return;
            }
            currPlaceableObj.GetComponent<BuildObject>().Init();
            currPlaceableObj = null;
        }
    }

    /// <summary>
    /// Rotates the floating furniture object.
    /// </summary>
    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y;
        currPlaceableObj.transform.Rotate(Vector3.up, mouseWheelRotation * 10f);
    }

    /// <summary>
    /// Moves the floating furniture object to follow the pointer position.
    /// </summary>
    private void MoveCurrPlaceableObjToMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Ground")))
        {
            currPlaceableObj.transform.localPosition = hitInfo.point;
            currPlaceableObj.transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    /// <summary>
    /// Sets the prefab to be placed.
    /// </summary>
    /// <param name="newPrefab">New prefab to be placed.</param>
    public void SetPrefab(GameObject newPrefab)
    {
        placeableObjPrefab = newPrefab;

        if (currPlaceableObj)
        {
            GameObject oldPlaceableObj = currPlaceableObj;
            currPlaceableObj = Instantiate(placeableObjPrefab);
            Destroy(oldPlaceableObj);
        }
    }

    /// <summary>
    /// Helper method to tell the system that the pointer is currently above UI.
    /// </summary>
    public void OnPointerEnterUI() { isOverUI = true; }

    /// <summary>
    /// Helper method to tell the system that the pointer is no longer above UI.
    /// </summary>
    public void OnPonterExitUI() { isOverUI = false; }

    // Overriden methods
    // See Mode.Activate()
    protected override void Activate()
    {
        base.Activate();
        furniturePanel.SetActive(true);
    }

    // See Mode.Deactivate()
    protected override void Deactivate()
    {
        base.Deactivate();
        furniturePanel.SetActive(false);
        Destroy(currPlaceableObj);
        isDestroying = false;
    }
}
