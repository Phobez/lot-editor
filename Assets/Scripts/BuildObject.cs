using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for placeable objects.
/// </summary>
public class BuildObject : MonoBehaviour
{
    [SerializeField]
    private int objId = 0;
    [SerializeField]
    private bool multipleRenderers = false;

    public bool IsHighlighted { get; private set; }

    // variables and references
    private List<Renderer> renderers;
    private new Renderer renderer;
    private Color oriColour;
    public bool Saveable { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {
        if (multipleRenderers)
        {
            renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
            if (renderers.Count > 0) oriColour = renderers[0].material.color;
        }
        else
        {
            if (GetComponent<Renderer>()) renderer = GetComponent<Renderer>();
            else renderer = GetComponentInChildren<Renderer>();
            oriColour = renderer.material.color;
        }
        IsHighlighted = false;
    }

    /// <summary>
    /// Marks the build object as saveable.
    /// </summary>
    public void Init() { Saveable = true; }

    /// <summary>
    /// Highlights the build object in the scene.
    /// </summary>
    public void Highlight()
    {
        if (multipleRenderers) foreach (Renderer r in renderers) r.material.color = Color.yellow;
        else renderer.material.color = Color.yellow;
        IsHighlighted = true;
    }

    /// <summary>
    /// Turns off highlighting for the build object in the scene.
    /// </summary>
    public void Dehighlight()
    {
        if (multipleRenderers) foreach (Renderer r in renderers) r.material.color = oriColour;
        else renderer.material.color = oriColour;
        IsHighlighted = false;
    }

    /// <summary>
    /// Sets the build object's colour.
    /// </summary>
    /// <param name="colour">Colour to be set.</param>
    public void SetColour(Color colour)
    {
        if (multipleRenderers) foreach (Renderer r in renderers) r.material.color = colour;
        else renderer.material.color = colour;
        oriColour = colour;
    }

    /// <summary>
    /// Returns the build object's colour.
    /// </summary>
    /// <returns>Build object's colour.</returns>
    public Color GetColour() { return oriColour; }

    /// <summary>
    /// Returns the build object's ID.
    /// </summary>
    /// <returns>Build object's ID.</returns>
    public int GetObjID() { return objId; }
}
