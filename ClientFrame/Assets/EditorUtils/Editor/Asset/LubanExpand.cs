
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEditor;

public class LubanExpand
{
   
    [MenuItem("Luban/Export Excel")]
    public static void ExecuteBat()
    {
       
        // 获取上一级目录
        var parentPath = Path.GetDirectoryName(Path.GetDirectoryName(Application.dataPath));
        // 确定 .bat 文件的绝对路径
        var batFilePath = Path.Combine(parentPath, "Luban/DataTables/gen.bat").Replace("\\", "/" );
        // 获取.bat文件所在目录
        var batDirectory = Path.GetDirectoryName(batFilePath);
        if (!File.Exists(batFilePath))
        {
            UnityEngine.Debug.LogError($"Bat file not found: {batFilePath}");
            return;
        }
        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = batFilePath,
                UseShellExecute = false,          // 不使用系统Shell
                RedirectStandardOutput = true,    // 重定向输出
                RedirectStandardError = true,     // 重定向错误
                CreateNoWindow = false,            // 不创建新窗口
                WorkingDirectory = batDirectory   // 关键点：工作目录设为.bat所在文件夹
            };

            var process = new Process { StartInfo = processInfo };
            process.Start();

            // 读取输出（防止进程阻塞）
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            AssetDatabase.Refresh();
            if (!string.IsNullOrEmpty(output))
                UnityEngine.Debug.Log($"Bat Output:\n{output}");

            if (!string.IsNullOrEmpty(error))
                UnityEngine.Debug.LogError($"Bat Error:\n{error}");

            UnityEngine.Debug.Log("Bat executed successfully!");
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError($"Failed to execute bat: {e.Message}");
        }
    }
}
