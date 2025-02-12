using UnityEngine;

public class LayerShop:LayerBase
{
    public void OnHomeClick()
    {
        UIManager.Instance.SwitchLayer(WindowID.LayerHome);
    }
}
