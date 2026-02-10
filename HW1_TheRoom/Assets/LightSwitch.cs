using UnityEngine;
using UnityEngine.InputSystem;

public class LightSwitch : MonoBehaviour
{
    public new Light light;
    public InputActionReference action;
    private void Start()
    {
        light = GetComponent<Light>();
        action.action.Enable();
        action.action.performed += (ctx) =>
        {
            light.color = new Color(Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f),
                                    Random.Range(0.0f, 1.0f));
        };
    }
}
