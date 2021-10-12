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
            if (currPlaceableObj == null)
            {
                currPlaceableObj = Instantiate(placeableObjPrefab);
                currPlaceableObj.transform.SetPositionAndRotation(new Vector3(currPlaceableObj.transform.localPosition.x, currPlaceableObj.transform.localPosition.y, currPlaceableObj.transform.localPosition.z), Quaternion.identity);
            }

            ray = cam.ScreenPointToRay(Input.mousePosition);

            // if pointing at a wall
            if (Physics.Raycast(ray, out hitInfo, 1000f, LayerMask.GetMask("Wall")))
            {
                // deactive the floating door
                if (currPlaceableObj.activeInHierarchy) currPlaceableObj.SetActive(false);

                // instantiate or set active the door wall
                if (currDoorWall == null) currDoorWall = Instantiate(doorWallPrefab);
                else if (!currDoorWall.activeInHierarchy) currDoorWall.SetActive(true);

                // if there is no current wall OR the current door wall's position is not at the wall
                if (!currWall || !currDoorWall.transform.localPosition.Equals(currWall.transform.localPosition))
                {
                    // if there is a current wall, set it to true
                    if (currWall) currWall.SetActive(true);

                    // replaces the current wall with a door wall
                    currWall = hitInfo.collider.gameObject;
                    currDoorWall.transform.SetPositionAndRotation(currWall.transform.localPosition, currWall.transform.localRotation);
                    currWall.SetActive(false);
                }

                ReleaseIfClicked();
            }
            // if not pointing at a wall
            else
            {
                // turn on the floating door
                if (!currPlaceableObj.activeInHierarchy) currPlaceableObj.SetActive(true);

                // turn off the door wall
                if (currDoorWall) currDoorWall.SetActive(false);

                // turn on the wall
                if (currWall) currWall.SetActive(true);

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
            currPlaceableObj.transform.localPosition = hitInfo.point;
            currPlaceableObj.transform.localRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    // Overriden methods
    // See Mode.Deactivate()
    protected override void Deactivate()
    {
        Destroy(currPlaceableObj);
        Destroy(currDoorWall);
    }
}
