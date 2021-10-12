using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controls behaviour of the Colour Mode.
/// </summary>
public class ColourMode : Mode
{
    // inspector fields
    [SerializeField]
    private GameObject colourPicker = null;

    // variables and references
    private string[] layerMasks = { "Wall", "Door Wall", "Object" };
    private float[] colours = { 0, 0, 0 };

    public UnityEvent OnSelectObj;
    private Camera cam = null;
    private GameObject SelectedObj
    {
        get
        {
            return selectedObj;
        }

        set
        {
            selectedObj = value;

            if (SelectedObj)
            {
                oriColor = SelectedObj.GetComponent<BuildObject>().GetColour();
                colours[0] = oriColor.r;
                colours[1] = oriColor.g;
                colours[2] = oriColor.b;
                OnSelectObj.Invoke();
            }
        }
    }
    private GameObject selectedObj = null;
    private GameObject hoveredObj = null;

    private RaycastHit hitInfo;
    private Color oriColor;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (OnSelectObj == null) OnSelectObj = new UnityEvent();
    }

    private void Update()
    {
        if (isActive)
        {

            if (Input.GetMouseButtonDown(0)) SelectObject();
            else
            {
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000f, LayerMask.GetMask(layerMasks)))
                {
                    if (hitInfo.collider.gameObject != hoveredObj && hitInfo.collider.gameObject != SelectedObj)
                    {
                        if (hoveredObj) hoveredObj.GetComponent<BuildObject>().Dehighlight();
                        hoveredObj = hitInfo.collider.gameObject;
                        hoveredObj.GetComponent<BuildObject>().Highlight();
                    }
                }
                else
                {
                    if (hoveredObj)
                    {
                        hoveredObj.GetComponent<BuildObject>().Dehighlight();
                        hoveredObj = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Selects the object being pointed at.
    /// </summary>
    private void SelectObject()
    {
        if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hitInfo, 1000f, LayerMask.GetMask(layerMasks)))
        {
            if (hitInfo.collider.gameObject == hoveredObj) hoveredObj.GetComponent<BuildObject>().Dehighlight();

            SelectedObj = hitInfo.collider.gameObject;
        }
    }

    /// <summary>
    /// Changes the colour of the selected object.
    /// </summary>
    /// <param name="rgbIndex">The index of the colour to be changed. 0 - red, 1 - green, 2 - blue.</param>
    /// <param name="colourFloat">Value of the colour to be changed.</param>
    public void ChangeColour(int rgbIndex, float colourFloat)
    {
        colours[rgbIndex] = colourFloat;
        Color tempColour = GetColour();
        SelectedObj.GetComponent<BuildObject>().SetColour(tempColour);
    }

    /// <summary>
    /// Gets the current colour of the selected object.
    /// </summary>
    /// <returns>Current colour of the selected object.</returns>
    public Color GetColour() { return new Color(colours[0], colours[1], colours[2]); }

    // Overriden methods
    // See Mode.Activate()
    protected override void Activate()
    {
        base.Activate();
        if (colourPicker) colourPicker.SetActive(true);
        else Debug.LogError("Colour mode's colour picker is not assigned!");
    }

    // See Mode.Deactivate()
    protected override void Deactivate()
    {
        base.Deactivate();
        SelectedObj = null;
        if (colourPicker) colourPicker.SetActive(false);
    }
}
