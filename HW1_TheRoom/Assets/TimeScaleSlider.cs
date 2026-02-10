using UnityEngine;
using UnityEngine.UI;

public class TimeScaleSlider : MonoBehaviour
{
    public Slider slider;
    public CelestialTimeController timeController;

    void Start()
    {
        slider.value = timeController.timeScale;
        slider.onValueChanged.AddListener(OnSliderChanged);
    }

    void OnSliderChanged(float value)
    {
        timeController.timeScale = value;
    }
}
