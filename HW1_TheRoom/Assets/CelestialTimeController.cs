using UnityEngine;

public class CelestialTimeController : MonoBehaviour
{
    [Header("Time Scale")]
    [Range(0f, 10f)]
    public float timeScale = 1f;

    public static float TimeScale { get; private set; } = 1f;

    void Update()
    {
        TimeScale = timeScale;
    }
}
