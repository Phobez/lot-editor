using UnityEngine;

/// <summary>
/// Activates the list of tips for a specific mode.
/// </summary>
public class TipActivator : MonoBehaviour
{
    // inspector field
    [SerializeField]
    private Mode mode = null;
    [SerializeField]
    private GameObject baseTips = null;

    // Start is called before the first frame update
    private void Start()
    {
        mode.OnActivate.AddListener(delegate { OnActivate(); });
        mode.OnDeactivate.AddListener(delegate { OnDeactivate(); });
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Callback for when a mode is activated. Activates the game object and deactivates the base tips.
    /// </summary>
    private void OnActivate()
    {
        gameObject.SetActive(true);
        baseTips.SetActive(false);
    }

    /// <summary>
    /// Callback for when a mode is deactivated. Deactivates the game object and activates the base tips.
    /// </summary>
    private void OnDeactivate()
    {
        gameObject.SetActive(false);
        baseTips.SetActive(true);
    }
}
