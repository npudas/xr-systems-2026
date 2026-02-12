using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    // This script should be attached to both controller objects in the scene
    // Make sure to define the input in the editor (LeftHand/Grip and RightHand/Grip recommended respectively)
    CustomGrab otherHand = null;
    public List<Transform> nearObjects = new List<Transform>();
    public Transform grabbedObject = null;
    public InputActionReference action;
    bool grabbing = false;
    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private Rigidbody grabbedRb;
    private bool originalUseGravity;
    private bool originalIsKinematic;
    private Vector3 previousFramePosition;
    private Vector3 releaseVelocity;
    public InputActionReference precisionRotateAction;

    private void Start()
    {
        action.action.Enable();
        precisionRotateAction.action.Enable();

        // Initialize previous position and rotation
        previousPosition = transform.position;
        previousRotation = transform.rotation;
        
        // Find the other hand
        foreach(CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    void Update()
    {
        grabbing = action.action.IsPressed();
        if (grabbing)
        {
            // Grab nearby object or the object in the other hand
            if (!grabbedObject)
            {
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;
                if (grabbedObject)
                {
                    grabbedRb = grabbedObject.GetComponent<Rigidbody>();

                    if (grabbedRb)
                    {
                        // Store original state
                        originalUseGravity = grabbedRb.useGravity;
                        originalIsKinematic = grabbedRb.isKinematic;

                        // Only override if it was dynamic
                        if (!originalIsKinematic)
                        {
                            grabbedRb.useGravity = false;
                            grabbedRb.isKinematic = true;
                        }
                    }
                }
            }
            if (grabbedObject)
            {
                // Change these to add the delta position and rotation instead
                // Save the position and rotation at the end of Update function, so you can compare previous pos/rot to current here
                Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(previousRotation);
                Vector3 offset = grabbedObject.position - previousPosition;
                offset = deltaRotation * offset;

                grabbedObject.position = transform.position + offset;
                float multiplier = precisionRotateAction.action.IsPressed() ? 2f : 1f;

                // Convert deltaRotation to axis-angle
                deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

                // Multiply angle
                Quaternion amplifiedRotation = Quaternion.AngleAxis(angle * multiplier, axis);

                grabbedObject.rotation = amplifiedRotation * grabbedObject.rotation;
            }
        }
        // If let go of button, release object
        else if (grabbedObject)
        {
            if (grabbedRb)
            {
                // Restore original state
                grabbedRb.useGravity = originalUseGravity;
                grabbedRb.isKinematic = originalIsKinematic;
                grabbedRb.linearVelocity = releaseVelocity;
            }

            grabbedObject = null;
            grabbedRb = null;
        }

        // Should save the current position and rotation here
        previousPosition = transform.position;
        previousRotation = transform.rotation;

        releaseVelocity = (transform.position - previousFramePosition) / Time.deltaTime;
        previousFramePosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure to tag grabbable objects with the "grabbable" tag
        // You also need to make sure to have colliders for the grabbable objects and the controllers
        // Make sure to set the controller colliders as triggers or they will get misplaced
        // You also need to add Rigidbody to the controllers for these functions to be triggered
        // Make sure gravity is disabled though, or your controllers will (virtually) fall to the ground

        Transform t = other.transform;
        if(t && t.tag.ToLower()=="grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        Transform t = other.transform;
        if( t && t.tag.ToLower()=="grabbable")
            nearObjects.Remove(t);
    }
}
