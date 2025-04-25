using System;
using UnityEngine;

public class ScrollViewExpandCell : MonoBehaviour
{
    public int showIndex;
    public int dataIndex;
    protected ScrollViewExpand Sc;
    public void Init(ScrollViewExpand sc, int showIdx, int dataIdx)
    {
        Sc = sc;
        showIndex = showIdx;
        dataIndex = dataIdx;
    }
   
    
    public virtual void OnSelect(int index)
    {
        
    }
}
