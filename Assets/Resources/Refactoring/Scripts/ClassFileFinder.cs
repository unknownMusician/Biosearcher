using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

namespace Biosearcher.Refactoring
{
    [NeedsRefactor(Needs.Optimization)]
    internal static class ClassFileFinder
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
            string typeName = type.Name;
            if (type.IsGenericType)
            {
                int genericBracketIndex = typeName.IndexOf("`");
                if (genericBracketIndex != -1)
                {
                    typeName = typeName.Substring(0, genericBracketIndex + 1);
                }
            }
            if (type.IsClass)
            {
                return $"class {typeName}";
            }
            else if (type.IsInterface)
            {
                return $"interface {typeName}";
            }
            else if (type.IsEnum)
            {
                return $"enum {typeName}";
            }
            else
            {
                return $"struct {typeName}";
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
}