using UnityEngine;

/// <summary>
/// Mode to build doors on walls.
/// </summary>
public class DoorMode : Mode
{
    // inspector fields
    [SerializeField]
    private GameObject placeableObjPrefab = null;
    [SerializeField]
    private GameObject doorWallPrefab = null;

    // variables and references
    private Camera cam = null;
    private GameObject currPlaceableObj = null;
    private GameObject currDoorWall = null;
    private GameObject currWall = null;
    private Ray ray;
    private RaycastHit hitInfo;
    private float mouseWheelRotation;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive)
        {
            if (currPlaceableObj == null) currPlaceableObj = Instantiate(placeableObjPrefab);

            ray = cam.ScreenPointToRay(Input.mousePosition);

            // if pointing at a wall
            if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Wall")))
            {
                // deactive the floating door
                if (currPlaceableObj.activeInHierarchy) currPlaceableObj.SetActive(false);

                if (!hitInfo.collider.GetComponent<BuildObject>().IsHighlighted)
                {
                    if (currWall) currWall.GetComponent<BuildObject>().Dehighlight();
                    currWall = hitInfo.collider.gameObject;
                    currWall.GetComponent<BuildObject>().Highlight();
                }

                ReleaseIfClicked();
            }
            // if not pointing at a wall
            else
            {
                // turn on the floating door
                if (!currPlaceableObj.activeInHierarchy) currPlaceableObj.SetActive(true);

                // turn on the wall
                if (currWall && currWall.GetComponent<BuildObject>().IsHighlighted) currWall.GetComponent<BuildObject>().Dehighlight();

                MoveCurrPlaceableObjToMouse();
            }
        }
    }

    /// <summary>
    /// Places a door wall when the left mouse button is clicked.
    /// </summary>
    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currDoorWall = Instantiate(doorWallPrefab);
            currDoorWall.transform.SetPositionAndRotation(currWall.transform.localPosition, currWall.transform.localRotation);
            Destroy(currWall);
            currWall = null;
            currDoorWall.GetComponent<BuildObject>().Init();
            currDoorWall = null;
        }
    }

    /// <summary>
    /// Moves the placeable door wall following the pointer.
    /// </summary>
    private void MoveCurrPlaceableObjToMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Ground")))
        {
            Vector3 newPos = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.5f, hitInfo.point.z);
            currPlaceableObj.transform.localPosition = newPos;
            currPlaceableObj.transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    // Overriden methods
    // See Mode.Deactivate()
    protected override void Deactivate()
    {
        base.Deactivate();
        Destroy(currPlaceableObj);
        Destroy(currDoorWall);
    }
}
