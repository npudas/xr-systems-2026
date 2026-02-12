using UnityEngine;

public class FloorReset : MonoBehaviour
{
    public ItemSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        InspectableItem item = other.GetComponent<InspectableItem>();
        if (item)
        {
            spawner.SpawnNew();
        }
    }
}
