using UnityEngine;

public class ViewBase : UIBase
{
    
    public override void Close()
    {
        base.Close();
        UIManager.Instance.CloseUI(windowID);
    }
}
