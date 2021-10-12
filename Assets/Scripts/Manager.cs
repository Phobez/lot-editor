using UnityEngine;

/// <summary>
/// Manages edit modes.
/// </summary>
public class Manager : MonoBehaviour
{
    public enum Modes
    {
        None,
        Build,
        Furniture,
        Door,
        Colour
    }

    // inspector fields
    [SerializeField]
    private KeyCode buildModeHotkey = KeyCode.B;
    [SerializeField]
    private KeyCode furnitureModeHotkey = KeyCode.F;
    [SerializeField]
    private KeyCode doorModeHotkey = KeyCode.D;
    [SerializeField]
    private KeyCode colourModeHotkey = KeyCode.C;

    // variables and references
    public static Modes CurrMode { get; private set; }

    private Mode buildMode = null;
    private Mode furnitureMode = null;
    private Mode doorMode = null;
    private Mode colourMode = null;
    private Mode activeMode = null;

    // Start is called before the first frame update
    private void Start()
    {
        CurrMode = Modes.None;
        buildMode = GetComponent<BuildMode>();
        furnitureMode = GetComponent<FurnitureMode>();
        doorMode = GetComponent<DoorMode>();
        colourMode = GetComponent<ColourMode>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(buildModeHotkey))
        {
            if (CurrMode == Modes.Build) DeactivateMode();
            else ActivateMode(Modes.Build);
        }
        else if (Input.GetKeyDown(furnitureModeHotkey))
        {
            if (CurrMode == Modes.Furniture) DeactivateMode();
            else ActivateMode(Modes.Furniture);
        }
        else if (Input.GetKeyDown(doorModeHotkey))
        {
            if (CurrMode == Modes.Door) DeactivateMode();
            else ActivateMode(Modes.Door);
        }
        else if (Input.GetKeyDown(colourModeHotkey))
        {
            if (CurrMode == Modes.Colour) DeactivateMode();
            else ActivateMode(Modes.Colour);
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    /// <summary>
    /// Activates a mode.
    /// </summary>
    /// <param name="mode">Mode to be activated.</param>
    private void ActivateMode(Modes mode)
    {
        DeactivateMode();

        switch (mode)
        {
            case Modes.Build:
                activeMode = buildMode;
                break;
            case Modes.Furniture:
                activeMode = furnitureMode;
                break;
            case Modes.Door:
                activeMode = doorMode;
                break;
            case Modes.Colour:
                activeMode = colourMode;
                break;
        }

        CurrMode = mode;
        activeMode.IsActive = true;
    }

    /// <summary>
    /// Deactivates the current mode.
    /// </summary>
    public void DeactivateMode()
    {
        if (activeMode)
        {
            activeMode.IsActive = false;
            activeMode = null;
            CurrMode = Modes.None;
        }
    }
}
