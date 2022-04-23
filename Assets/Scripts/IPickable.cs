using UnityEngine;

internal interface IPickable : IInteractibleObj
{
    public void OnPick();
    public void OnDrop();
    public Transform Anchor { get; set; }
    public void TryAnchor();
}