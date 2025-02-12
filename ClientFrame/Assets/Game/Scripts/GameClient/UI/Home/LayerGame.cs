using UnityEngine;

public class LayerGame:LayerBase
{
    public void OnHomeClick()
    {
        UIManager.Instance.SwitchLayer(WindowID.LayerHome);
    }
    
}
