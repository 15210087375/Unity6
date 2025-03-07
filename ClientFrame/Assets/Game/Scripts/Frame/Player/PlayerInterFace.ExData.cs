using UnityEngine;

public partial class PlayerInterFace
{
    
    public static void SetExData(int key, int value)
    {
        Player.Instance.ExData.SetExData(key, value);
    }
    public static int GetExData(int key)
    {
        return  Player.Instance.ExData.GetExData(key);
    }
    
    public static void ClearExData(int key)
    {
        Player.Instance.ExData.ResetExData(key);
    }
}
