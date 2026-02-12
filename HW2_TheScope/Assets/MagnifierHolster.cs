using UnityEngine;

public class MagnifierHolster : MonoBehaviour
{
    public CustomGrab leftHand;
    public CustomGrab rightHand;

    public Transform toolbeltAnchor;

    public float returnSpeed = 5f;
    public float rotateSpeed = 5f;
    public float snapDistance = 0.05f;

    private bool IsGrabbed()
    {
        return leftHand.grabbedObject == transform ||
               rightHand.grabbedObject == transform;
    }

    void Update()
    {
        if (!IsGrabbed())
        {
            ReturnToHolster();
        }
    }

    void ReturnToHolster()
    {
        if (!toolbeltAnchor) return;

        // Smooth position
        transform.position = Vector3.Lerp(
            transform.position,
            toolbeltAnchor.position,
            Time.deltaTime * returnSpeed
        );

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            toolbeltAnchor.rotation,
            Time.deltaTime * rotateSpeed
        );

        // Snap when very close (prevents micro jitter)
        if (Vector3.Distance(transform.position, toolbeltAnchor.position) < snapDistance)
        {
            transform.position = toolbeltAnchor.position;
            transform.rotation = toolbeltAnchor.rotation;
        }
    }
}
