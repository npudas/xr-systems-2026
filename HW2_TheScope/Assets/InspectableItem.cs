using UnityEngine;

public class InspectableItem : MonoBehaviour
{
    public GameObject defectVisual;
    public bool IsDefective { get; private set; }
    public void RandomizeState()
    {
        IsDefective = Random.value > 0.5f;
        defectVisual?.SetActive(IsDefective);
    }
}
