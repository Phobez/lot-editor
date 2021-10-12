using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls colour picker slider behaviour.
/// </summary>
public class ColourSlider : MonoBehaviour
{
    // inspector fields
    [SerializeField]
    private ColourMode colourMode = null;
    [SerializeField]
    private int colourIndex = 0;
    [SerializeField]
    private Image img = null;

    // variables and references
    private Slider slider;

    // Start is called before the first frame update
    private void Start()
    {
        slider = GetComponent<Slider>();

        slider.onValueChanged.AddListener(delegate { OnValueChanged(); });

        colourMode.OnSelectObj.AddListener(delegate { OnSelectObj(); });
    }

    /// <summary>
    /// Changes the value of the slider to match the newly selected object's initial colour.
    /// </summary>
    private void OnSelectObj()
    {
        Color currColour = colourMode.GetColour();
        float colourVal = 0f;

        switch (colourIndex)
        {
            case 0:
                colourVal = currColour.r;
                break;
            case 1:
                colourVal = currColour.g;
                break;
            case 2:
                colourVal = currColour.b;
                break;
        }

        slider.value = colourVal;
    }

    /// <summary>
    /// Changes the colour of the selected object according to the slider value.
    /// </summary>
    private void OnValueChanged()
    {
        colourMode.ChangeColour(colourIndex, slider.value);
        if (img) img.color = colourMode.GetColour();
    }
}
