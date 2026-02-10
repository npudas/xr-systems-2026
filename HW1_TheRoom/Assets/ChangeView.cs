using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeView : MonoBehaviour
{
    [Header("References")]
    public XROrigin xrOrigin;
    public Transform outsideView;

    [Header("Action")]
    public InputActionProperty toggleView;

    private Vector3 savedPosition;
    private Quaternion savedRotation;
    private bool inside = true;

    private void OnEnable()
    {
        toggleView.action.Enable();
        toggleView.action.performed += OnToggleView;
    }

    private void OnDisable()
    {
        toggleView.action.performed -= OnToggleView;
        toggleView.action.Disable();
    }

    void OnToggleView(InputAction.CallbackContext ctx)
    {
        if (inside)
        {
            savedPosition = xrOrigin.transform.position;
            savedRotation = xrOrigin.transform.rotation;

            xrOrigin.transform.SetPositionAndRotation(
                outsideView.position,
                outsideView.rotation
            );
            
            inside = false;
        } else
        {
            xrOrigin.transform.SetPositionAndRotation(
                savedPosition,
                savedRotation
            );
            inside = true;
        }
    }
}
