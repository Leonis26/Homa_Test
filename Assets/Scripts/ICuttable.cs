using UnityEngine;

internal interface ICuttable : IInteractibleObj
{
    //public void Init(int _slices, Vector3 _interDir);
    public Material InsideMat { get; set; }

    public int RemainingSplits { get; }
    public void OnCut();
}