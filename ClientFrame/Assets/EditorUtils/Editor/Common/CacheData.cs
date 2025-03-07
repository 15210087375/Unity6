using UnityEditor;
using UnityEngine;

public class CacheData 
{
   
    //写一个工具，路径是Tools/清除数据
    [MenuItem("Tools/清除数据")]
    public static void ClearData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        PlayerInterFace.ClearData();
        Debug.Log("清除数据成功");
    }
}
