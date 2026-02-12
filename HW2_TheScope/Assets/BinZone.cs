using UnityEngine;

public class BinZone : MonoBehaviour
{
    public bool acceptsDefective; // true = defective bin
    public ScoreManager scoreManager;
    public ItemSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        InspectableItem item = other.GetComponent<InspectableItem>();

        if (item != null)
        {
            bool correct = item.IsDefective == acceptsDefective;

            scoreManager.RegisterResult(correct);

            spawner.SpawnNew();
        }
    }
}
