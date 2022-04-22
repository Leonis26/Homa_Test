using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSlotsManager : MonoBehaviour
{
    public List<Transform> ObjSlots { get; private set; } = new List<Transform>();

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
            ObjSlots.Add(transform.GetChild(i));
    }
    
    public Transform GetEmptySlot()
    {
        foreach(var s in ObjSlots)
        {
            if (s.childCount == 0)
                return s;
        }
        return null;
    }
}
