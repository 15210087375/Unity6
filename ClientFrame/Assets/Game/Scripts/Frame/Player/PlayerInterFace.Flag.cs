using System.Text;
using UnityEngine;

public partial class PlayerInterFace 
{
    public static void SetFlag(int flag)
    {
        if (GameDefine.GameEnv == GameEnv.Local)
        {
            Player.Instance.Flag.SetFlag(flag);
        }
      
    }
    public static void GetFlag(int flag,out int value)
    {
        if (GameDefine.GameEnv == GameEnv.Local)
        {
            value = Player.Instance.Flag.GetFlag(flag);
        }
        else
        {
            value = 0;
        }
    }
    public static void ClearFlag(int flag)
    {
        if (GameDefine.GameEnv == GameEnv.Local)
        {
            Player.Instance.Flag.CleanFlag(flag);
        }
    }
    
    
   
    
   
}
