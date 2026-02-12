using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public InspectableItem itemPrefab;
    public Transform spawnPoint;
    private InspectableItem currentItem;
    private Rigidbody currentRb;

    void Start()
    {
        SpawnNew();
    }

    public void SpawnNew()
    {
        if (currentItem == null)
        {
            currentItem = Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            currentItem.transform.position = spawnPoint.position;
            currentItem.transform.rotation = spawnPoint.rotation;
        }

        currentItem.RandomizeState();
        currentRb = currentItem.GetComponent<Rigidbody>();
        currentRb.linearVelocity = Vector3.zero;
        currentRb.angularVelocity = Vector3.zero;
    }

    public InspectableItem GetCurrentItem()
    {
        return currentItem;
    }
}
