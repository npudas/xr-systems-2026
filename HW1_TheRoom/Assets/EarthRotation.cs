using UnityEngine;

public class EarthRotation : MonoBehaviour
{
    [Header("Earth Rotation")]
    [Tooltip("Rotation speed of the Earth in degrees per second")]
    public float rotationPeriod = 10f;  // 10 seconds = 1 day

    [Tooltip("Axial tilt in degrees")]
    public float axialTilt = 23.44f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Apply Earth's axial tilt
        transform.rotation = Quaternion.Euler(axialTilt, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        float degreesPerSecond = 360f / rotationPeriod;
        transform.Rotate(Vector3.up, degreesPerSecond * Time.deltaTime * CelestialTimeController.TimeScale, Space.Self);
    }
}
