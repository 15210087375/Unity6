using UnityEngine;

public partial class PlayerInterFace
{
    
    public static void SetExData(int key, int value)
    {
        if (GameDefine.GameEnv == GameEnv.Local)
        {
            Player.Instance.ExData.SetExData(key, value);
        }
    }
    public static void GetExData(int key, out int value)
    {
        value = 0;
        if (GameDefine.GameEnv == GameEnv.Local)
        {
            value = Player.Instance.ExData.GetExData(key);
        }
    }
    
    public static void ClearExData(int key)
    {
        if (GameDefine.GameEnv == GameEnv.Local)
        {
            Player.Instance.ExData.ResetExData(key);
        }
    }
}
