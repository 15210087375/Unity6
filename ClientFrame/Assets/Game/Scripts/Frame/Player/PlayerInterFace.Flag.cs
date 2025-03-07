using System.Text;
using UnityEngine;

public partial class PlayerInterFace 
{
    public static void SetFlag(int flag)
    {
        Player.Instance.Flag.SetFlag(flag);
      
    }
    public static int GetFlag(int flag)
    {
        return Player.Instance.Flag.GetFlag(flag);
    }
    public static void ClearFlag(int flag)
    {
        Player.Instance.Flag.CleanFlag(flag);
    }
    
    
   
    
   
}
