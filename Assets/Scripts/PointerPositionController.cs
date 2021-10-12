using UnityEngine;

/// <summary>
/// Controls the position of the build mode pointer.
/// </summary>
public class PointerPositionController : MonoBehaviour
{
    // inspector fields
    [SerializeField]
    private GameObject pointer = null;
    [SerializeField]
    private float interval = 1f;

    // variables and references
    public bool IsActive { get; private set; }

    private Camera cam = null;
    private Vector3 currPos = Vector3.zero;
    private Vector3 lastPos = Vector3.zero;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsActive)
        {
            currPos = SnapPos(GetWorldPoint());

            pointer.transform.SetPositionAndRotation(currPos, Quaternion.identity);

            if (!currPos.Equals(lastPos)) lastPos = currPos;
        }
    }

    /// <summary>
    /// Gets the world coordinates of where the mouse pointer is pointing at.
    /// </summary>
    /// <returns>The world point if found, (0, 0, 0) otherwise.</returns>
    public Vector3 GetWorldPoint()
    {
        if (cam)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Ground")))
            {
                return hit.point;
            }

            return Vector3.zero;
        }
        else
        {
            Debug.LogError("The reference to Camera 'cam' has not been asigned.");
            return Vector3.zero;
        }
    }

    /// <summary>
    /// Snaps the position to the nearest valid grid point.
    /// </summary>
    /// <param name="oriPos">Position to be snapped.</param>
    /// <returns>Snapped position.</returns>
    public Vector3 SnapPos(Vector3 oriPos)
    {
        Vector3 snappedPos = Vector3.zero;

        float newX = FloorByInterval(oriPos.x);
        float newY = Mathf.Floor(oriPos.y + 0.5f);
        float newZ = FloorByInterval(oriPos.z);

        snappedPos.Set(newX, newY, newZ);

        return snappedPos;
    }


    /// <summary>
    /// Rounds f to the nearest multiplication of the interval.
    /// </summary>
    /// <param name="f">Float to be rounded.</param>
    /// <returns>The float rounded to the nearest multiplication of the interval.</returns>
    private float FloorByInterval(float f)
    {
        if (Mathf.Approximately(interval, 1f)) return Mathf.Floor(f + 0.5f);

        if (f < 0) return (int)((f - (interval / 2)) / interval) * interval;
        return (int)((f + (interval / 2)) / interval) * interval;
    }

    /// <summary>
    /// Activates the pointer.
    /// </summary>
    public void Activate()
    {
        pointer.SetActive(true);
        IsActive = true;
    }

    /// <summary>
    /// Deactivates the poointer.
    /// </summary>
    public void Deactivate()
    {
        pointer.SetActive(false);
        IsActive = false;
    }
}
