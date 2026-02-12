using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;

public sealed class LensCameraSwitcher : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform lens;
    public Transform lensCamera;
    [Header("Camera offset")]
    public float offset = 0f;

    void Update()
    {
        
        if (xrOrigin == null || xrOrigin.Camera == null)
        return;

        Transform playerCam = xrOrigin.Camera.transform;

        Vector3 viewerDir = playerCam.position - lens.position;

        float distance = Mathf.Clamp(viewerDir.magnitude, 0f, 0.5f);

        Vector3 mirroredPosition =
            lens.position - lens.forward * distance;

        lensCamera.SetPositionAndRotation(
            mirroredPosition,
            playerCam.rotation
        );
    }
}
