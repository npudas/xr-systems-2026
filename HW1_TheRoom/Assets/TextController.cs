using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TextController : MonoBehaviour
{
    [Header("References")]
    public XROrigin xrOrigin;
    public Transform earth;
    public Transform moon;
    public Transform iss;
    public Transform earthText;
    public Transform moonText;
    public Transform issText;

    [Header("Text Offset")]
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    // Update is called once per frame
    void Update()
    {
        // Update positions and facings
        earthText.transform.SetPositionAndRotation(
            earth.position + offset,
            GetFacing(earth.position)
        );
        moonText.transform.SetPositionAndRotation(
            moon.position + offset,
            GetFacing(moon.position)
        );
        issText.transform.SetPositionAndRotation(
            iss.position + offset,
            GetFacing(iss.position)
        );

    }

    quaternion GetFacing(Vector3 position)
    {
        if (xrOrigin == null || xrOrigin.Camera == null)
            return quaternion.identity;

        Vector3 cameraPosition = xrOrigin.Camera.transform.position;

        // Direction from text to camera
        Vector3 direction = cameraPosition - position;

        // Prevent NaNs if we’re exactly on top of the camera
        if (direction.sqrMagnitude < 0.0001f)
            return quaternion.identity;

        // Create a rotation that faces the camera
        Quaternion unityRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Flip 180° because Canvas/Text faces -Z by default
        unityRotation *= Quaternion.Euler(0f, 180f, 0f);

        // Convert to Unity.Mathematics quaternion
        return unityRotation;
    }
}
