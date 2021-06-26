#define DIRECTORY_READER_PROFILING

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if DIRECTORY_READER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.Refactoring.FileInput
{
    public class DirectoryReader 
    {
        public static void GetFilePaths(out List<string> changedFilePaths, out List<string> notChangedFilePaths, Dictionary<string, FileInfo> fileInfos)
        {
#if DIRECTORY_READER_PROFILING
            Profiler.BeginSample(nameof(GetFilePaths));
#endif

            var changedFilePathsList = new List<string>();
            var notChangedFilePathsList = new List<string>();
            ForeachScriptFile(filePath => AddFilePath(filePath, changedFilePathsList, notChangedFilePathsList, fileInfos));
            changedFilePaths = changedFilePathsList;
            notChangedFilePaths = notChangedFilePathsList;

#if DIRECTORY_READER_PROFILING
            Profiler.EndSample();
#endif
        }
        public static List<string> GetFilePaths()
        {
#if DIRECTORY_READER_PROFILING
            Profiler.BeginSample(nameof(GetFilePaths));
#endif

            var filePathsList = new List<string>();

            ForeachScriptFile(filePathsList.Add);

#if DIRECTORY_READER_PROFILING
            Profiler.EndSample();
#endif

            return filePathsList;
        }

        public static void AddFilePath(string filePath, List<string> changedFilePaths, List<string> notChangedFilePaths, Dictionary<string, FileInfo> fileInfos)
        {
#if DIRECTORY_READER_PROFILING
            Profiler.BeginSample(nameof(AddFilePath));
#endif

            if (fileInfos.TryGetValue(filePath, out FileInfo fileInfo) && fileInfo.writeTime == File.GetLastWriteTime(filePath))
            {
                notChangedFilePaths.Add(filePath);
            }
            else
            {
                changedFilePaths.Add(filePath);
            }

#if DIRECTORY_READER_PROFILING
            Profiler.EndSample();
#endif
        }

        public static void ForeachScriptFile(Action<string> action) => ForeachScriptFile(Application.dataPath, action);
        public static void ForeachScriptFile(string startDir, Action<string> action)
        {
#if DIRECTORY_READER_PROFILING
            Profiler.BeginSample(nameof(ForeachScriptFile));
#endif

            try
            {
                foreach (string file in Directory.GetFiles(startDir))
                {
                    if (file.EndsWith(".cs"))
                    {
                        action.Invoke(file);
                    }
                }
                foreach (string dir in Directory.GetDirectories(startDir))
                {
                    ForeachScriptFile(dir, action);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

#if DIRECTORY_READER_PROFILING
            Profiler.EndSample();
#endif
        }

    }
}