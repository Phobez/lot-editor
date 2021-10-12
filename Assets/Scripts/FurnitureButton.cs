using UnityEngine;

/// <summary>
/// Component to hold information for furniture object buttons.
/// </summary>
public class FurnitureButton : MonoBehaviour
{
    // inspector fields
    [SerializeField]
    private FurnitureMode mode = null;
    [SerializeField]
    private GameObject prefab = null;

    /// <summary>
    /// Sets the prefab used in furniture mode.
    /// </summary>
    public void SetPrefab()
    {
        mode.SetPrefab(prefab);
    }
}
