using UnityEngine;
using UnityEngine.InputSystem;
using Unity.XR.CoreUtils;

public class FollowISS : MonoBehaviour
{
    [Header("References")]
    public XROrigin xrOrigin;
    public Transform iss;

    [Header("Input")]
    public InputActionProperty toggleFollowAction;

    [Header("Follow Settings")]
    public Vector3 followOffset = new Vector3(0f, 0.5f, -1.5f);
    public float rotationSmoothing = 5f;

    bool isFollowing = false;

    Vector3 savedPosition;
    Quaternion savedRotation;

    Vector3 lastISSPosition;
    Vector3 issVelocity;

    void OnEnable()
    {
        toggleFollowAction.action.Enable();
        toggleFollowAction.action.performed += ToggleFollow;
    }

    void OnDisable()
    {
        toggleFollowAction.action.performed -= ToggleFollow;
        toggleFollowAction.action.Disable();
    }

    void Start()
    {
        if (iss != null)
            lastISSPosition = iss.position;
    }

    void Update()
    {
        if (!isFollowing || iss == null)
            return;

        UpdateISSVelocity();
        UpdateFollowView();
    }

    void ToggleFollow(InputAction.CallbackContext ctx)
    {
        if (!isFollowing)
        {
            // Save current XR Origin transform
            savedPosition = xrOrigin.transform.position;
            savedRotation = xrOrigin.transform.rotation;

            isFollowing = true;
        }
        else
        {
            // Restore previous view
            xrOrigin.transform.SetPositionAndRotation(savedPosition, savedRotation);
            isFollowing = false;
        }
    }

    void UpdateISSVelocity()
    {
        issVelocity = (iss.position - lastISSPosition) / Time.deltaTime * CelestialTimeController.TimeScale;
        lastISSPosition = iss.position;
    }

    void UpdateFollowView()
    {
        // Determine forward direction from velocity
        Vector3 forward = issVelocity.normalized;
        if (forward.sqrMagnitude < 0.0001f)
            forward = iss.forward;

        Quaternion targetRotation = Quaternion.LookRotation(forward, Vector3.up);

        // Position XR Origin relative to ISS
        Vector3 targetPosition =
            iss.position +
            targetRotation * followOffset;

        xrOrigin.transform.position = targetPosition;

        // Smooth rotation for comfort
        xrOrigin.transform.rotation = Quaternion.Slerp(
            xrOrigin.transform.rotation,
            targetRotation,
            rotationSmoothing * Time.deltaTime
        );
    }
}
