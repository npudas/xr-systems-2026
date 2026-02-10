using System.Collections.Generic;
using UnityEngine;

public class MoonOrbit : MonoBehaviour
{
    [Header("Orbit")]
    public Transform earth;
    public float orbitPeriod = 30f; // seconds per orbit
    public float orbitInclination = 5.14f;

    [Header("Ellipse")]
    public float semiMajorAxis = 5f; // ie mean distance from earth
    public float eccentricity = 0.055f;

    [Header("Tidal Lock")]
    public bool tidalLock = true;

    [Header("Trail")]
    public int trailLength = 200;
    public float trailSpacing = 0.05f;

    float orbitAngle;
    LineRenderer lineRenderer;
    List<Vector3> trailPoints = new List<Vector3>();
    Vector3 lastTrailPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

        UpdateOrbit();
        UpdateTrail();
    }

    void UpdateOrbit()
    {
        orbitAngle += (360f / orbitPeriod) * Time.deltaTime * CelestialTimeController.TimeScale;
        orbitAngle %= 360f;

        float rad = orbitAngle * Mathf.Deg2Rad;
        float a = semiMajorAxis;
        float b = a * Mathf.Sqrt(1 - eccentricity * eccentricity);

        float x = a * Mathf.Cos(rad);
        float z = b * Mathf.Sin(rad);

        transform.localPosition = new Vector3(x, 0f, z);

        if (tidalLock)
        {
            transform.LookAt(earth.position);
        }
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
