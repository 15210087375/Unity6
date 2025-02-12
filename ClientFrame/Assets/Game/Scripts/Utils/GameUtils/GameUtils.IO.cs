
//工具类

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

public static partial class GameUtils
{
    public static class  IO
    {
        public static string[] ReadFileLine(string filePath)
        {
            using (var tr = new StreamReader(filePath))
            {
                var s = tr.ReadToEnd();
                var ret = s.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                return ret;
            }
        }
        
        public static string ReadFile(string file)
        {
            var path = Path.Combine(GameDefine.CacheResPath, file);
            using (var tr = new StreamReader(path))
            {
                var s = tr.ReadToEnd();
                return s;
            }
        }
        
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">完整目录</param>
        /// <param name="data"></param>
        /// <param name="append"></param>
        public static void WritePath(string filePath, string data,bool append = false)
        {
            using (var file = new StreamWriter(filePath, append, Encoding.UTF8))
            {
                file.Write(data);
                file.Flush();
                Logger.Info($"WriteFile {filePath} OK");
            }
        }

        /// <summary>
        /// 写文件，到缓存目录
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="data"></param>
        /// <param name="append"></param>
        public static void WriteFile(string filePath, string data, bool append = false)
        {
            var path = Path.Combine(GameDefine.CacheResPath, filePath);
            WritePath(path, data, append);
        }
        
        //把对象序列化为字符串
        public static string Serialize<T>(T obj)
        {
            try
            {
                var formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Close();
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 把字符串反序列化为对象OperateFile
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="str"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T Deserialize<T>( string str)
        {
            try
            {
                var formatter = new BinaryFormatter();
                byte[] buffer = Convert.FromBase64String(str);
                MemoryStream stream = new MemoryStream(buffer);
                var obj = (T) formatter.Deserialize(stream);
                stream.Flush();
                stream.Close();
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public static T Load<T>(string file)
        {
            var path = Path.Combine(GameDefine.CacheResPath, file);
         
            if (File.Exists(path) == false)
            {
                return default(T);
            }
            var s= ReadFile(file);

            var ret = Deserialize<T>(s);
            return ret;
        }

       
    }
}

public static partial class GameExtensions
{
    //数据类型扩展  函数第一个参数为 静态this类型 
}