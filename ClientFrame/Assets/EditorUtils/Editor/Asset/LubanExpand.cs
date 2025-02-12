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
        var batFilePath = Path.Combine(parentPath, "Luban/DataTables/gen.bat").Replace("\\", "/" );
       
        if (File.Exists(batFilePath))
        {
            // 配置 ProcessStartInfo
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = "cmd.exe"; // 使用 cmd.exe 打开命令行
            processInfo.Arguments = $"/C \"{batFilePath}\""; // /C 参数让 cmd 执行完后关闭窗口
            processInfo.UseShellExecute = false;      // 必须为 false 才能设置 Redirect 标志
            processInfo.RedirectStandardOutput = true; // 获取输出信息
            processInfo.RedirectStandardError = true;  // 获取错误信息
            processInfo.CreateNoWindow = false;       // 打开一个控制台窗口
            try
            {
                var process = Process.Start(processInfo);
                if (process != null)
                {
                    process.WaitForExit(); // 等待进程结束
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
