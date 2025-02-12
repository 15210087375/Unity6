using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IView
{
    public void OnInit(object data = null);
    public void OnOpen(object data = null);
    

    public void OnClose();
    
}
