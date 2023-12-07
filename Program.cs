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
                Console.WriteLine("请输入一个Minecraft游戏文件夹：");
                string? indexesPath = Console.ReadLine() + "\\assets\\indexes\\";
                string[] indexes = Directory.GetFiles(indexesPath);
                Console.WriteLine();
                foreach (string item in indexes)
                {
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
                }
                Console.WriteLine("请选择您想复制的版本（输入对应版本号）：");
                string? fn = Console.ReadLine();
                string path = fn switch
                {
                    "1.20" => indexesPath + "5.json",
                    "1.20.2" => indexesPath + "8.json",
                    _ => indexesPath + fn + ".json",
                };
                Console.WriteLine("请输入复制的目标文件夹路径：");
                string? copyPath = Console.ReadLine();
                CopyAll(path, copyPath);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void CopyAll(string path, string copyPath)
        {
            try
            {
                string json = new StreamReader(path).ReadToEnd();
                JObject? text = JsonConvert.DeserializeObject<JObject>(json);
                if (text?["objects"] is JObject obj)
                {
                    foreach (KeyValuePair<string, JToken?> jsonObj in obj)
                    {
                        if (jsonObj.Value is JObject childObj)
                        {
                            string? hash = childObj["hash"]?.ToString();
                            string[]? strings = path?.Split('\\');
                            string objPath = path?.Substring(0, path.Length - (1 + strings[^1].Length + strings[^2].Length)) + "objects\\";
                            DirectoryInfo directory = new(objPath);
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

        public static void CopyDesignative(string path, string copyPath)
        {
            try
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}