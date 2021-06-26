using Biosearcher.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Profiling;

namespace Biosearcher.Refactoring
{
    [NeedsRefactor]
    internal static class RefactorManager
    {
        private static Dictionary<string, FileInfo> fileInfos;
        public static Log[] Logs { get; private set; } = new Log[0];

        private static RefactorSettings RefactorSettings => Resources.Load<RefactorSettings>("Settings/Refactor Settings");

        private const BindingFlags MembersFlags =
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;


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
            Debug.Log("HandleSettingsChanged");
            OnChange(fileInfos == null ? (Action)FillFileInfos : () => { });
        }

        private static void OnChange(Action fileUpdateAction)
        {
            Profiler.BeginSample($"RefactorManager.{nameof(OnChange)}");
            try
            {
                RefactorSettings.Parameters parameters = RefactorSettings.GetParamsSafe();
                if (parameters.enabled)
                {
                    fileUpdateAction();

                    Logs = fileInfos.SelectMany(pair => pair.Value.logs).ToArray();
                    if (parameters.showInConsole)
                    {
                        LogAll(Logs);
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
            Profiler.EndSample();
        }

        private static void FillFileInfos()
        {
            Profiler.BeginSample(nameof(FillFileInfos));

            List<string> filePaths = GetFilePaths();

            IEnumerable<SearchedTypeInfo> changedTypes = GetSearchedTypeInfos();

            fileInfos = GetChangedFileInfos(filePaths, changedTypes);

            Profiler.EndSample();
        }
        private static void UpdateFileInfos()
        {
            Profiler.BeginSample(nameof(UpdateFileInfos));

            // get <changed> filePaths and <notChanged> filePaths
            GetFilePaths(out List<string> changedFilePaths, out List<string> notChangedFilePaths);

            // get <changed> Types (new Types that are outside <notChanged> files)
            List<SearchedTypeInfo> changedTypes = GetChangedTypes(notChangedFilePaths);

            // search for <changed> Types in <changed> filePaths and create <newLogs>
            Dictionary<string, FileInfo> changedFileInfos = GetChangedFileInfos(changedFilePaths, changedTypes);

            // get <notChanged> Logs from <notChanged> filePaths and add <newLogs> to them
            fileInfos = CombineLogs(changedFileInfos, notChangedFilePaths);

            Profiler.EndSample();
        }

        private static Dictionary<string, FileInfo> CombineLogs(IDictionary<string, FileInfo> changedLogs, IEnumerable<string> notChangedFilePaths)
        {
            Profiler.BeginSample(nameof(CombineLogs));

            var newLogs = new Dictionary<string, FileInfo>(changedLogs);

            foreach (string notChangedFilePath in notChangedFilePaths)
            {
                newLogs.Add(notChangedFilePath, fileInfos[notChangedFilePath]);
            }

            Profiler.EndSample();

            return newLogs;
        }
        private static Dictionary<string, FileInfo> GetChangedFileInfos(IEnumerable<string> changedFilePaths, IEnumerable<SearchedTypeInfo> changedTypes)
        {
            Profiler.BeginSample(nameof(GetChangedFileInfos));

            var newFileInfos = new Dictionary<string, FileInfo>();

            string previousLine;
            string line;
            int lineNumber;

            foreach (string filePath in changedFilePaths)
            {
                //string fileBody = File.ReadAllText(filePath);
                using var fileReader = new StreamReader(filePath);

                var searchedTypes = new Dictionary<int, SearchedTypeInfo>();
                var memberInfos = new Dictionary<int, List<SearchedMemberInfo>>();
                var foundTypes = new Dictionary<int, FoundTypeInfo>();

                int counter = 0;
                foreach (SearchedTypeInfo type in changedTypes)
                {
                    searchedTypes.Add(counter++, type);
                }

                previousLine = "";
                lineNumber = 1;

                while ((line = fileReader.ReadLine()) != null)
                {
                    InspectLine(line, previousLine, lineNumber, filePath, searchedTypes, memberInfos, foundTypes);

                    previousLine = line;
                    lineNumber++;
                }

                List<FoundTypeInfo> foundTypeInfos = foundTypes.Select(pair => pair.Value).ToList();
                List<Log> logs = ToLogs(foundTypeInfos);

                var fileInfo = new FileInfo
                {
                    writeTime = File.GetLastWriteTime(filePath),
                    types = foundTypeInfos,
                    logs = logs
                };

                newFileInfos.Add(filePath, fileInfo);
            }

            Profiler.EndSample();

            return newFileInfos;
        }

        private static void InspectLine(string line, string previousLine, int lineNumber, string filePath,
            Dictionary<int, SearchedTypeInfo> searchedTypes, Dictionary<int, List<SearchedMemberInfo>> memberInfos,
            Dictionary<int, FoundTypeInfo> foundTypes)
        {
            Profiler.BeginSample(nameof(InspectLine));

            if (previousLine.Contains("NeedsRefactor") || line.Contains("NeedsRefactor"))
            {
                InspectLineForTitles(line, lineNumber, filePath, searchedTypes, memberInfos, foundTypes);
                InspectLineForMembers(line, lineNumber, memberInfos, foundTypes);
            }

            Profiler.EndSample();
        }

        private static void InspectLineForTitles(string line, int lineNumber, string filePath,
            Dictionary<int, SearchedTypeInfo> searchedTypes, Dictionary<int, List<SearchedMemberInfo>> memberInfos,
            Dictionary<int, FoundTypeInfo> foundTypes)
        {
            Profiler.BeginSample(nameof(InspectLineForTitles));

            List<int> needToRemoveFromSearchedTypes = new List<int>();

            foreach ((int index, SearchedTypeInfo info) in searchedTypes)
            {
                int columnNumber = line.IndexOf(GetTextToSearch(info.typeInfo.type));
                if (columnNumber != -1)
                {
                    memberInfos.Add(index, info.memberInfos);
                    foundTypes.Add(index, new FoundTypeInfo
                    {
                        typeInfo = new FoundTitleInfo
                        {
                            info = info.typeInfo,
                            lineNumber = lineNumber,
                            columnNumber = columnNumber
                        },
                        memberInfos = new List<FoundMemberInfo>(),
                        filePath = filePath
                    });
                    needToRemoveFromSearchedTypes.Add(index);
                }
            }
            needToRemoveFromSearchedTypes.Foreach(index => searchedTypes.Remove(index));

            Profiler.EndSample();
        }

        private static void InspectLineForMembers(string line, int lineNumber,
            Dictionary<int, List<SearchedMemberInfo>> memberInfos, Dictionary<int, FoundTypeInfo> foundTypes)
        {
            Profiler.BeginSample(nameof(InspectLineForMembers));

            var needToRemoveFromMemberInfos = new Dictionary<int, List<SearchedMemberInfo>>();

            foreach ((int index, List<SearchedMemberInfo> infos) in memberInfos)
            {
                foreach (SearchedMemberInfo info in infos)
                {
                    int columnNumber = line.IndexOf(info.member.Name);
                    if (columnNumber != -1)
                    {
                        needToRemoveFromMemberInfos.SafeAddToValueCollection(index, info);
                        foundTypes[index].memberInfos.Add(new FoundMemberInfo
                        {
                            info = info,
                            columnNumber = columnNumber,
                            lineNumber = lineNumber
                        });
                    }
                }
            }
            foreach ((int index, List<SearchedMemberInfo> infos) in needToRemoveFromMemberInfos)
            {
                if (needToRemoveFromMemberInfos[index].Count == memberInfos[index].Count)
                {
                    memberInfos.Remove(index);
                }
                else
                {
                    infos.Foreach(info => memberInfos[index].Remove(info));
                }
            }

            Profiler.EndSample();
        }

        private static List<Log> ToLogs(List<FoundTypeInfo> foundTypeInfos)
        {
            Profiler.BeginSample(nameof(ToLogs));

            var logs = new List<Log>();
            foreach (FoundTypeInfo info in foundTypeInfos)
            {
                logs.Add(
                    new Log(
                        GetLogText(info.typeInfo.info.type, info.typeInfo.info.attribute),
                        info.filePath,
                        info.typeInfo.info.type.Namespace,
                        info.typeInfo.lineNumber,
                        info.typeInfo.columnNumber));
                info.memberInfos
                    .Foreach(member => logs.Add(
                        new Log(
                            GetLogText(info.typeInfo.info.type, member.info.member.Name, member.info.attribute),
                            info.filePath,
                            info.typeInfo.info.type.Namespace,
                            member.lineNumber,
                            member.columnNumber)));
            }

            Profiler.EndSample();

            return logs;
        }

        //private static void AppendFullTypeLogsIfNeeded(Type type, string filePath, List<Log> logs)
        //{
        //    Profiler.BeginSample("AppendFullTypeLogsIfNeeded");

        //    if (type.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
        //    {
        //        logs.Add(new Log(GetLogText(type, attribute), filePath));
        //    }
        //    type.GetMembers(MembersFlags).Foreach(m => AppendMemberLogIfNeeded(m, type, filePath, logs));

        //    Profiler.EndSample();
        //}
        //private static void AppendMemberLogIfNeeded(MemberInfo member, Type memberParent, string filePath, List<Log> logs)
        //{
        //    Profiler.BeginSample("AppendMemberLogIfNeeded");

        //    if (member.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
        //    {
        //        logs.Add(new Log(GetLogText(memberParent, member.Name, attribute), filePath));
        //    }

        //    Profiler.EndSample();
        //}
        private static string GetTextToSearch(Type type)
        {
            Profiler.BeginSample(nameof(GetTextToSearch));

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
                Profiler.EndSample();

                return $"class {typeName}";
            }
            else if (type.IsInterface)
            {
                Profiler.EndSample();

                return $"interface {typeName}";
            }
            else if (type.IsEnum)
            {
                Profiler.EndSample();

                return $"enum {typeName}";
            }
            else
            {
                Profiler.EndSample();

                return $"struct {typeName}";
            }
        }

        private static List<SearchedTypeInfo> GetChangedTypes(IEnumerable<string> notChangedFilePaths)
        {
            Profiler.BeginSample(nameof(GetChangedTypes));

            var types = new List<SearchedTypeInfo>();
            bool typeFound;
            foreach (SearchedTypeInfo type in GetSearchedTypeInfos())
            {
                typeFound = false;
                foreach (string notChangedFilePath in notChangedFilePaths)
                {
                    if (fileInfos[notChangedFilePath].types.Select(type => type.typeInfo.info.type).Contains(type.typeInfo.type))
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

            Profiler.EndSample();

            return types;
        }

        private static void GetFilePaths(out List<string> changedFilePaths, out List<string> notChangedFilePaths)
        {
            Profiler.BeginSample(nameof(GetFilePaths));

            var changedFilePathsList = new List<string>();
            var notChangedFilePathsList = new List<string>();
            ForeachScriptFile(filePath => AddFilePath(filePath, changedFilePathsList, notChangedFilePathsList));
            changedFilePaths = changedFilePathsList;
            notChangedFilePaths = notChangedFilePathsList;

            Profiler.EndSample();
        }
        private static List<string> GetFilePaths()
        {
            Profiler.BeginSample(nameof(GetFilePaths));

            var filePathsList = new List<string>();

            ForeachScriptFile(filePathsList.Add);

            Profiler.EndSample();

            return filePathsList;
        }

        private static void AddFilePath(string filePath, List<string> changedFilePaths, List<string> notChangedFilePaths)
        {
            Profiler.BeginSample(nameof(AddFilePath));

            if (fileInfos.TryGetValue(filePath, out FileInfo fileInfo) && fileInfo.writeTime == File.GetLastWriteTime(filePath))
            {
                notChangedFilePaths.Add(filePath);
            }
            else
            {
                changedFilePaths.Add(filePath);
            }

            Profiler.EndSample();
        }


        private static void ForeachScriptFile(Action<string> action) => ForeachScriptFile(Application.dataPath, action);
        private static void ForeachScriptFile(string startDir, Action<string> action)
        {
            Profiler.BeginSample(nameof(ForeachScriptFile));

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

            Profiler.EndSample();
        }

        private static IEnumerable<Type> GetAllTypes()
        {
            Profiler.BeginSample(nameof(GetAllTypes));

            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.Namespace != null && t.Namespace.Contains(nameof(Biosearcher)));

            Profiler.EndSample();

            return types;
        }
        private static IEnumerable<SearchedTypeInfo> GetSearchedTypeInfos()
        {
            Profiler.BeginSample(nameof(GetSearchedTypeInfos));

            IEnumerable<SearchedTypeInfo> searchedTypeInfos = GetAllTypes()
                .Select(GetSearchedTypeInfoOrNull)
                .Where(info => info != null)
                .Cast<SearchedTypeInfo>();

            Profiler.EndSample();

            return searchedTypeInfos;
        }
        private static SearchedTypeInfo? GetSearchedTypeInfoOrNull(Type type)
        {
            Profiler.BeginSample(nameof(GetSearchedTypeInfoOrNull));

            if (!type.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
            {
                Profiler.EndSample();

                return null;
            }

            List<SearchedMemberInfo> searchedMemberInfos = new List<SearchedMemberInfo>(
                type.GetMembers(MembersFlags)
                .Select(GetSearchedMemberInfoOrNull)
                .Where(info => info != null)
                .Cast<SearchedMemberInfo>());

            var searchedTypeInfo = new SearchedTypeInfo
            {
                typeInfo = new SearchedTitleInfo { type = type, attribute = attribute },
                memberInfos = searchedMemberInfos
            };

            Profiler.EndSample();

            return searchedTypeInfo;
        }
        private static SearchedMemberInfo? GetSearchedMemberInfoOrNull(MemberInfo member)
        {
            Profiler.BeginSample(nameof(GetSearchedMemberInfoOrNull));

            if (!member.TryGetCustomAttribute(out NeedsRefactorAttribute attribute))
            {
                Profiler.EndSample();

                return null;
            }

            var searchedTypeMemberInfo = new SearchedMemberInfo
            {
                member = member,
                attribute = attribute
            };

            Profiler.EndSample();

            return searchedTypeMemberInfo;
        }

        private static string GetLogText(Type type, NeedsRefactorAttribute attribute)
        {
            return $"<b><color=#99ccff>{type.Namespace}</color>. <color=#99ff99>{type.Name}</color></b> needs <color=#ff6699>{attribute.NeededAction}</color>.";
        }
        private static string GetLogText(Type type, string memberName, NeedsRefactorAttribute attribute)
        {
            return $"<b><color=#99ccff>{type.Namespace}</color>. <color=#99ff99>{type.Name}</color>. " +
                $"<color=#ffff99>{memberName}</color></b> needs <color=#ff6699>{attribute.NeededAction}</color>.";
        }


        private static void LogAll(IEnumerable<Log> logs)
        {
            Profiler.BeginSample(nameof(LogAll));

            logs.Foreach(log => Debug.LogWarningFormat(log.Text));

            Profiler.EndSample();
        }
    }

    [NeedsRefactor]
    internal struct FileInfo
    {
        public DateTime writeTime;
        public List<FoundTypeInfo> types;
        public List<Log> logs;
    }

    [NeedsRefactor]
    internal struct SearchedTypeInfo
    {
        public SearchedTitleInfo typeInfo;
        public List<SearchedMemberInfo> memberInfos;
    }

    [NeedsRefactor]
    internal struct FoundTypeInfo
    {
        public FoundTitleInfo typeInfo;
        public List<FoundMemberInfo> memberInfos;
        public string filePath;
    }

    [NeedsRefactor]
    internal struct SearchedTitleInfo
    {
        public Type type;
        public NeedsRefactorAttribute attribute;
    }

    [NeedsRefactor]
    internal struct FoundTitleInfo
    {
        public SearchedTitleInfo info;
        public int lineNumber;
        public int columnNumber;
    }

    [NeedsRefactor]
    internal struct SearchedMemberInfo
    {
        public MemberInfo member;
        public NeedsRefactorAttribute attribute;
    }

    [NeedsRefactor]
    internal struct FoundMemberInfo
    {
        public SearchedMemberInfo info;
        public int lineNumber;
        public int columnNumber;
    }
}