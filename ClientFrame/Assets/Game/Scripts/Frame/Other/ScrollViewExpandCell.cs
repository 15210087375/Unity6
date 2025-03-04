using System;
using UnityEngine;

public class ScrollViewExpandCell : MonoBehaviour
{
    public int showIndex;
    public int dataIndex;
    protected ScrollViewExpand1 Sc;
    public void Init(ScrollViewExpand1 sc, int showIdx, int dataIdx)
    {
        Sc = sc;
        showIndex = showIdx;
        dataIndex = dataIdx;
    }
   
    
    public virtual void OnSelect(int index)
    {
        
    }
}
