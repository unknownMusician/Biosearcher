using Biosearcher.Refactoring.FileInput;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if BIOSEARCHER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.Refactoring
{
    internal static class RefactorManager
    {
        private static Dictionary<string, FileInfo> fileInfos;
        public static Log[] Logs { get; private set; } = new Log[0];

        private static RefactorSettings RefactorSettings => Resources.Load<RefactorSettings>("Settings/Refactor Settings");


        [UnityEditor.InitializeOnLoadMethod]
        public static void OnUnityStart()
        {
            OnChange(FillFileInfos);
        }
        [NeedsRefactor(Needs.Implementation)]
        private static void OnFilesChanged()
        {
            OnChange(UpdateFileInfos);
        }
        public static void HandleSettingsChanged()
        {
            OnChange(fileInfos == null ? (Action)FillFileInfos : () => { });
        }

        private static void OnChange(Action fileUpdateAction)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample($"RefactorManager.{nameof(OnChange)}");
#endif

            try
            {
                RefactorSettings.Parameters parameters = RefactorSettings.GetParamsSafe();
                if (parameters.enabled)
                {
                    fileUpdateAction();

                    Logs = fileInfos.SelectMany(pair => pair.Value.logs).ToArray();
                    if (parameters.showInConsole)
                    {
                        Logger.LogAll(Logs);
                    }
                }
                else
                {
                    Logs = new Log[0];
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
        }

        private static void FillFileInfos()
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(FillFileInfos));
#endif

            List<string> filePaths = DirectoryReader.GetFilePaths();

            IEnumerable<SearchedTypeInfo> changedTypes = ReflectionHelper.GetSearchedTypeInfos();

            fileInfos = GetChangedFileInfos(filePaths, changedTypes);

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
        }
        private static void UpdateFileInfos()
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(UpdateFileInfos));
#endif

            DirectoryReader.GetFilePaths(out List<string> changedFilePaths, out List<string> notChangedFilePaths, fileInfos);

            List<SearchedTypeInfo> changedTypes = GetChangedTypes(notChangedFilePaths);

            Dictionary<string, FileInfo> changedFileInfos = GetChangedFileInfos(changedFilePaths, changedTypes);

            fileInfos = CombineLogs(changedFileInfos, notChangedFilePaths);

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif
        }

        private static Dictionary<string, FileInfo> CombineLogs(IDictionary<string, FileInfo> changedLogs, IEnumerable<string> notChangedFilePaths)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(CombineLogs));
#endif

            var newLogs = new Dictionary<string, FileInfo>(changedLogs);

            foreach (string notChangedFilePath in notChangedFilePaths)
            {
                newLogs.Add(notChangedFilePath, fileInfos[notChangedFilePath]);
            }

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return newLogs;
        }
        private static Dictionary<string, FileInfo> GetChangedFileInfos(IEnumerable<string> changedFilePaths, IEnumerable<SearchedTypeInfo> changedTypes)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetChangedFileInfos));
#endif

            var newFileInfos = new Dictionary<string, FileInfo>();

            foreach (string filePath in changedFilePaths)
            {
                FileInfo fileInfo = FileTypeSearcher.GetFileInfo(filePath, changedTypes, Logger.ToLogs);

                newFileInfos.Add(filePath, fileInfo);
            }

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return newFileInfos;
        }
        
        private static List<SearchedTypeInfo> GetChangedTypes(IEnumerable<string> notChangedFilePaths)
        {
#if BIOSEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetChangedTypes));
#endif

            var types = new List<SearchedTypeInfo>();
            bool typeFound;
            foreach (SearchedTypeInfo type in ReflectionHelper.GetSearchedTypeInfos())
            {
                typeFound = false;
                foreach (string notChangedFilePath in notChangedFilePaths)
                {
                    if (fileInfos[notChangedFilePath].typeInfos.Select(type => type.titleInfo.titleInfo.type).Contains(type.titleInfo.type))
                    {
                        typeFound = true;
                        break;
                    }
                }
                if (!typeFound)
                {
                    types.Add(type);
                }
            }

#if BIOSEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return types;
        }
    }
}