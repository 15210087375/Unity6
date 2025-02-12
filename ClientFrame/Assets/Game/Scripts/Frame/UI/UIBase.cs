using UnityEngine;

public abstract class UIBase : MonoBehaviour,IView
{
    //窗口ID
    [HideInInspector]
    public WindowID windowID;
    //常驻窗口
    [HideInInspector]
    public bool isResident = false;
    
   
    
    public void OnInit(object data = null)
    {
       
    }

    public void OnOpen(object data = null)
    {
     
    }
    public virtual void Close()
    {
        
    }
    public virtual void OnClose()
    {
        
    }
    
    
}
