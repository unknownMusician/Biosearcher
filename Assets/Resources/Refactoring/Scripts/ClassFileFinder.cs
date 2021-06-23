using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace Biosearcher.Refactoring
{
    public static class ClassFileFinder
    {
        private static List<string> classFiles;

        public static string FindTypePathOrDefault(Type type)
        {
            if(!TryFindTypePath(type, out string typePath))
            {
                Debug.LogWarning("Failed to lookup type file for type " + type.Name);
            }
            return typePath;
        }

        public static bool TryFindTypePath(Type type, out string typePath)
        {
            classFiles = new List<string>();
            FindAllScriptFiles(Application.dataPath);

            if (TryFindInFileNames(type.Name, out typePath))
            {
                return true;
            }
            if (TryFindInFileBodies(type, out typePath))
            {
                return true;
            }
            return false;
        }

        private static bool TryFindInFileNames(string typeName, out string typePath)
        {
            for (int i = 0; i < classFiles.Count; i++)
            {
                if (classFiles[i].Contains(typeName))
                {
                    typePath = classFiles[i];
                    return true;
                }
            }
            typePath = default;
            return false;
        }

        private static bool TryFindInFileBodies(Type type, out string typePath)
        {
            string textToSearch = GetTextToSearch(type);
            string codeFile;
            for (int i = 0; i < classFiles.Count; i++)
            {
                codeFile = File.ReadAllText(classFiles[i]);
                if (codeFile.Contains(textToSearch))
                {
                    typePath = classFiles[i];
                    return true;
                }
            }
            typePath = default;
            return false;
        }

        private static string GetTextToSearch(Type type)
        {
            if (type.IsClass)
            {
                return $"class {type.Name}";
            }
            else if (type.IsInterface)
            {
                return $"interface {type.Name}";
            }
            else if (type.IsEnum)
            {
                return $"enum {type.Name}";
            }
            else
            {
                return $"struct {type.Name}";
            }
        }

        private static void FindAllScriptFiles(string startDir)
        {
            try
            {
                foreach (string file in Directory.GetFiles(startDir))
                {
                    if (file.EndsWith(".cs"))
                    {
                        classFiles.Add(file);
                    }
                }
                foreach (string dir in Directory.GetDirectories(startDir))
                {
                    FindAllScriptFiles(dir);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
    }

    public sealed class TypeFileDetails
    {
        public string TypeName { get; private set; }
        public string Path { get; private set; }

        internal TypeFileDetails(string typeName, string path)
        {
            TypeName = typeName;
            Path = ToAssetPath(path);
        }

        private string ToAssetPath(string globalPath)
        {
            return globalPath.Substring(globalPath.IndexOf("Assets"));
        }
    }
}