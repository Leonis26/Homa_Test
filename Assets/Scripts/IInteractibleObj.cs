using UnityEngine;

internal interface IInteractibleObj
{
    public Vector3 InteractedDirection { get; }
    public Collider Col { get; }
    public Rigidbody Rb { get; }
    public Bounds SavedBounds { get; }
}