using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEditor;
public class LubanExpand
{
    [MenuItem("Luban/Export Excel")]
    public static void RunBatFile()
    {
        // 获取上一级目录
        var parentPath = Path.GetDirectoryName(Path.GetDirectoryName(Application.dataPath));
        // 确定 .bat 文件的绝对路径
        var batFilePath = Path.Combine(parentPath, "Luban/DataTables/gen.bat");
       
        if (File.Exists(batFilePath))
        {
            // 配置 ProcessStartInfo
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = batFilePath;  // 目标文件
            startInfo.UseShellExecute = false; // 禁用使用外壳
            startInfo.RedirectStandardOutput = true; // 重定向输出（如果需要查看输出）

            try
            {
                using (Process process = Process.Start(startInfo))
                {
                    // 可选择读取输出（在需要时）
                    using (StreamReader reader = process.StandardOutput)
                    {
                        string result = reader.ReadToEnd();
                        UnityEngine.Debug.Log(result);
                    }
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError("Failed to execute bat file: " + ex.Message);
            }
        }
        else
        {
            UnityEngine.Debug.LogError("The specified bat file does not exist: " + batFilePath);
        }
    }
}
