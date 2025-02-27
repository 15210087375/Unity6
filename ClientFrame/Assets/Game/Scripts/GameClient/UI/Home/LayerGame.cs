using UnityEngine;

public class LayerGame:LayerBase
{


    public void OnClickTestData()
    {
        UIManager.Instance.OpenView(WindowID.ViewTestData);
    }
    
}
