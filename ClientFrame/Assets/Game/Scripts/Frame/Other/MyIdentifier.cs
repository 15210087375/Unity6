using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyIdentifier : MonoBehaviour
{
    // [HideInInspector]
    public string identifier;

    public int index;
    public void SetIdentifier(string id)
    {
        identifier = id;
    }

    public void SetIndex(int idx)
    {
        index = idx;
    }
}