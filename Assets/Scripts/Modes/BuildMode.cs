using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mode to build and destroy walls.
/// </summary>
[RequireComponent(typeof(PointerPositionController))]
public class BuildMode : Mode
{
    // inspector fields
    [SerializeField]
    private GameObject wallPrefab = null;

    // variables and references
    private List<GameObject> currWalls = null;
    private PointerPositionController pointerPosController = null;
    private BuildObject highlightedWall = null;
    private Camera cam = null;
    private Vector3 lastPoint = Vector3.zero;
    private RaycastHit hitInfo;
    private bool isBuilding = false;
    private bool isDestroying = false;

    // Start is called before the first frame update
    protected void Start()
    {
        currWalls = new List<GameObject>();

        pointerPosController = GetComponent<PointerPositionController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive) GetInput();
    }

    /// <summary>
    /// Determines what to do based on input.
    /// </summary>
    private void GetInput()
    {
        if (isDestroying)
        {
            if (pointerPosController.IsActive) pointerPosController.Deactivate();

            if (Input.GetMouseButtonDown(0)) DeleteWall();
            else HighlightWall();
        }
        else
        {
            if (!pointerPosController.IsActive) pointerPosController.Activate();

            if (Input.GetMouseButtonDown(0)) StartWall();
            else if (Input.GetKey(KeyCode.Escape)) CancelWall();
            else if (Input.GetMouseButtonUp(0)) SetWall();
            else if (Input.GetKey(KeyCode.Delete)) isDestroying = true;
            else if (isBuilding) UpdateWall();
        }
    }

    /// <summary>
    /// Highlights the wall being pointed at.
    /// </summary>
    private void HighlightWall()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000f, LayerMask.GetMask("Wall", "Door Wall")))
        {
            if (highlightedWall == null || highlightedWall != hitInfo.collider.GetComponent<BuildObject>())
            {
                if (highlightedWall) highlightedWall.Dehighlight();
                highlightedWall = hitInfo.collider.GetComponent<BuildObject>();
            }

            if (!highlightedWall.IsHighlighted) highlightedWall.Highlight();
        }
    }

    /// <summary>
    /// Deletes the wall being pointed at.
    /// </summary>
    private void DeleteWall()
    {
        if (highlightedWall)
        {
            Destroy(highlightedWall.gameObject);
            highlightedWall = null;
        }
    }

    /// <summary>
    /// Begins construction of wall segments.
    /// </summary>
    private void StartWall()
    {
        isBuilding = true;

        Vector3 startPos = pointerPosController.GetWorldPoint();

        startPos = pointerPosController.SnapPos(startPos);
        startPos.Set(startPos.x, startPos.y + 0.25f, startPos.z);

        lastPoint = startPos;
    }

    /// <summary>
    /// Cancels construction of wall segments. Deletes all currently being constructed walls.
    /// </summary>
    private void CancelWall()
    {
        isBuilding = false;

        foreach (GameObject wall in currWalls)
        {
            Destroy(wall);
        }

        currWalls.Clear();
    }

    /// <summary>
    /// Finish construction of wall segments.
    /// </summary>
    private void SetWall()
    {
        isBuilding = false;

        currWalls.Clear();
    }

    /// <summary>
    /// Adds wall segment.
    /// </summary>
    private void UpdateWall()
    {
        Vector3 currPos = pointerPosController.GetWorldPoint();

        currPos = pointerPosController.SnapPos(currPos);
        currPos.Set(currPos.x, currPos.y + 0.25f, currPos.z);

        if (!currPos.Equals(lastPoint))
        {
            BuildSegment(currPos);
        }
    }

    /// <summary>
    /// Builds wall segment.
    /// </summary>
    /// <param name="currPos">Current snapped pointer position.</param>
    private void BuildSegment(Vector3 currPos)
    {
        if (!ValidatePos(currPos, lastPoint))
        {
            Debug.LogWarning("Cannot build wall between " + currPos + " and " + lastPoint + ".");
            return;
        }

        Vector3 midPoint = Vector3.Lerp(currPos, lastPoint, 0.5f);

        Collider[] hitColliders = Physics.OverlapSphere(midPoint, 0.1f, LayerMask.GetMask("Wall", "Door Wall", "Object"));

        if (hitColliders.Length > 0)
        {
            Debug.LogWarning("Cannot build wall at " + midPoint + ": There is already a wall there.");
            return;
        }

        GameObject newWall = Instantiate(wallPrefab, midPoint, Quaternion.identity);

        newWall.transform.LookAt(lastPoint);

        newWall.GetComponent<BuildObject>().Init();

        currWalls.Add(newWall);

        lastPoint = currPos;
    }

    /// <summary>
    /// Determines whether a wall segment can be built at currPos.
    /// </summary>
    /// <param name="currPos">Current snapped pointer position.</param>
    /// <param name="lastPoint">Last snapped pointer position.</param>
    /// <returns></returns>
    private bool ValidatePos(Vector3 currPos, Vector3 lastPoint)
    {
        if (currPos.x == lastPoint.x || currPos.z == lastPoint.z || (Mathf.Abs(currPos.x - lastPoint.x).Equals(currPos.z - lastPoint.z))) return true;

        return false;
    }

    // Overriden methods
    // See Mode.Activate()
    protected override void Activate()
    {
        base.Activate();
        pointerPosController.Activate();
        isDestroying = false;
    }

    // See Mode.Deactivate()
    protected override void Deactivate()
    {
        base.Deactivate();
        CancelWall();
        pointerPosController.Deactivate();
        highlightedWall = null;
    }
}
