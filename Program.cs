using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleTest
{
    public static partial class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please enter a Minecraft game path:"/* "请输入一个Minecraft游戏文件夹：" */);
                string? indexesPath = Console.ReadLine() + "\\assets\\indexes\\"; // 获取资源索引文件路径
                string[] indexes = Directory.GetFiles(indexesPath);               // 获取目录下的资源索引文件
                Console.WriteLine(); 
                foreach (string item in indexes)
                {
                    #region 将索引文件名格式化成版本号
                    switch (item.Split('\\')[^1])
                    {
                        case "1.json":
                            break;
                        case "2.json":
                            break;
                        case "3.json":
                            break;
                        case "4.json":
                            break;
                        case "5.json":
                            Console.WriteLine("1.20");
                            break;
                        case "6.json":
                            break;
                        case "7.json":
                            break;
                        case "8.json":
                            Console.WriteLine("1.20.2");
                            break;
                        default:
                            string[] versions = item.Split('\\')[^1].Split('.');
                            string version = "";
                            for (int i = 0; i < versions.Length - 1; i++)
                            {
                                version += versions[i];
                                if (i < versions.Length - 2)
                                {
                                    version += ".";
                                }
                            }
                            Console.WriteLine(version);
                            break;
                    }
                    #endregion
                }
                Console.WriteLine("Please select the version you want to copy (enter the corresponding version):"/* "请选择您想复制的版本（输入对应版本号）：" */);
                string? fn = Console.ReadLine();
                #region 将版本号反格式化成索引文件路径
                string path = fn switch
                {
                    "1.20" => indexesPath + "5.json",
                    "1.20.2" => indexesPath + "8.json",
                    _ => indexesPath + fn + ".json",
                };
                #endregion
                Console.WriteLine("Please enter the target path for the copy operation:"/* "请输入复制操作的目标文件夹路径：" */);
                string? copyPath = Console.ReadLine();
                CopyAll(path, copyPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void CopyAll(string path, string copyPath)
        {
            try
            {
                string json = new StreamReader(path).ReadToEnd();
                JObject? text = JsonConvert.DeserializeObject<JObject>(json);     // 将json文件反序列化
                if (text?["objects"] is JObject obj)
                {
                    foreach (KeyValuePair<string, JToken?> jsonObj in obj)        // 将json文件中objcets键下所有的子键遍历取出
                    {
                        if (jsonObj.Value is JObject childObj)
                        {
                            string? hash = childObj["hash"]?.ToString();          // 将json文件中objcets键下子键的一个值：hash提取出来
                            string[]? strings = path?.Split('\\');
                            string objPath = path?.Substring(0, path.Length - (1 + strings[^1].Length + strings[^2].Length)) + "objects\\"; // 通过硬编码获取资源路径
                            DirectoryInfo directory = new(objPath);
                            #region 将资源文件与索引文件的值做匹配，并复制到指定文件夹
                            foreach (FileInfo item in directory.GetFiles("*", SearchOption.AllDirectories))
                            {
                                string[] name = jsonObj.Key.Split('/');
                                if (item.Name == hash)
                                {
                                    string subDir = "";
                                    for (int i = 0; i <= name.Length - 2; i++)
                                    {
                                        subDir += name[i] + "\\";
                                    }
                                    Directory.CreateDirectory(copyPath + "\\" + subDir);
                                    File.Copy(item.FullName, copyPath + "\\" + subDir + name[^1]);
                                }
                            }
                            #endregion
                            Console.WriteLine(jsonObj.Key);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // public static void CopyDesignative(string path, string copyPath)
    }
}