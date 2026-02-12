using UnityEngine;
using Unity.XR.CoreUtils;

public class LensCameraController : MonoBehaviour
{
    public XROrigin xrOrigin;
    public Transform lensCenter;     // empty transform at center of lens
    public Transform lensCamera;

    [Header("Distance Behind Lens")]
    public float offset = 0.2f;
    public Renderer lens1Renderer;
    public Renderer lens2Renderer;

    void LateUpdate()
    {
        if (xrOrigin == null || xrOrigin.Camera == null || lensCenter == null || lensCamera == null)
            return;

        Transform playerCam = xrOrigin.Camera.transform;

        /*Vector3 P = playerCam.position;
        Vector3 L = lensCenter.position;

        // Direction from player to lens
        Vector3 direction = (L - P).normalized;

        // Position camera past the lens along that line
        Vector3 cameraPosition = L + direction * offset;

        // Face outward along same line
        Quaternion cameraRotation = Quaternion.LookRotation(direction, playerCam.up);

        lensCamera.SetPositionAndRotation(cameraPosition, cameraRotation);*/
        Vector3 direction = (lensCenter.position - playerCam.position).normalized;

        // Force camera up to match player up
        Quaternion camRot = Quaternion.LookRotation(direction, playerCam.up);

        lensCamera.SetPositionAndRotation(
            lensCenter.position + direction * offset,
            camRot
        );
        // Fix the lens view appearing rotated
        float roll = Vector3.SignedAngle(
            lensCenter.up,
            playerCam.up,
            direction
        );
         Debug.Log("Roll: " + roll);
        lens1Renderer.material.SetFloat("_RollCorrection", roll);
        lens2Renderer.material.SetFloat("_RollCorrection", roll);
    }
}
