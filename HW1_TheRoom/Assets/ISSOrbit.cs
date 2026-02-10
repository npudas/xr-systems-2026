using System.Collections.Generic;
using UnityEngine;

public class ISSOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform earth;        // Parent Earth
    public float orbitPeriod = 5f; // Seconds per orbit (fast!)
    public float orbitInclination = 51.6f; // ISS tilt in degrees
    
    [Header("Orbit Radius")]
    public float orbitRadius = 2f; // Local distance from Earth

    float orbitAngle;

    [Header("Trail")]
    public int trailLength = 200;
    public float trailSpacing = 0.05f;

    LineRenderer lineRenderer;
    List<Vector3> trailPoints = new List<Vector3>();
    Vector3 lastTrailPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (earth == null) earth = transform.parent;
        // Tilt the orbital plane
        transform.localRotation = Quaternion.Euler(orbitInclination, 0f, 0f);

        // Draw orbit trace
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
        }

        lastTrailPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (earth == null) return;
        UpdateTrail();

        // Advance orbit angle
        orbitAngle += (360f / orbitPeriod) * Time.deltaTime * CelestialTimeController.TimeScale;
        orbitAngle %= 360f;

        float rad = orbitAngle * Mathf.Deg2Rad;
        float x = orbitRadius * Mathf.Cos(rad);
        float z = orbitRadius * Mathf.Sin(rad);

        transform.localPosition = new Vector3(x, 0f, z);

        // Optional: Make ISS face forward along orbit
        transform.LookAt(earth.position);
    }

    void UpdateTrail()
    {
        if (lineRenderer == null) return;

        float distanceMoved = Vector3.Distance(transform.position, lastTrailPosition);

        if (distanceMoved >= trailSpacing)
        {
            trailPoints.Insert(0, transform.position);
            lastTrailPosition = transform.position;

            if (trailPoints.Count > trailLength)
            {
                trailPoints.RemoveAt(trailPoints.Count - 1);
            }

            lineRenderer.positionCount = trailPoints.Count;
            lineRenderer.SetPositions(trailPoints.ToArray());
        }
    }
}
