using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for edit modes.
/// </summary>
public class Mode : MonoBehaviour
{
    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
            if (isActive) Activate();
            else Deactivate();
        }
    }

    public UnityEvent OnActivate { get; protected set; }
    public UnityEvent OnDeactivate { get; protected set; }

    protected bool isActive = false;

    protected virtual void Awake() { OnActivate = new UnityEvent(); OnDeactivate = new UnityEvent(); }

    /// <summary>
    /// Invokes the OnActivate event. Override to modify behaviour on activation.
    /// </summary>
    protected virtual void Activate() { OnActivate.Invoke(); }

    /// <summary>
    /// Invokes the OnDeactivate event. Override to modify behaviour on deactivation.
    /// </summary>
    protected virtual void Deactivate() { OnDeactivate.Invoke(); }
}
