using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using cfg;
using Luban;
using UnityEngine;

public class TableManager:Singleton<TableManager>
{
    public Tables Tables;
    public void InitTable()
    {
        Tables = new Tables(LoadByteBuf);
    }

    private static ByteBuf LoadByteBuf(string file)
    {
        try
        {
            var filePath = $"{Application.dataPath}/Res/Table/{file}.bytes";
            using MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
            using MemoryMappedViewStream stream = mmf.CreateViewStream();
            byte[] buffer = new byte[4096]; // 4KB 缓冲区大小
            int bytesRead;
                    
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                // 处理读取的字节数据，例如将其写入另一个流或进行其他操作
                Console.WriteLine($"Bytes read: {bytesRead}");
            }
            return new ByteBuf(buffer);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the file: {ex.Message}");
            return null;
        }
       
    }
}
