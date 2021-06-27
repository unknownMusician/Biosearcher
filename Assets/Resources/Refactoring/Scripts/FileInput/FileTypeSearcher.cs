//#define FILE_TYPE_SEARCHER_PROFILING

using Biosearcher.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if FILE_TYPE_SEARCHER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.Refactoring.FileInput
{
    public static class FileTypeSearcher 
    {
        public static FileInfo GetFileInfo(string filePath, Dictionary<int, SearchedTypeInfo> searchedTypes, Func<List<FoundTypeInfo>, List<Log>> logsCreator)
        {
#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetFileInfo));
#endif

            List<FoundTypeInfo> typeInfos = TryFindTypeInfos(filePath, searchedTypes);

            FileInfo fileInfo = new FileInfo
            {
                writeTime = File.GetLastWriteTime(filePath),
                typeInfos = typeInfos,
                logs = logsCreator(typeInfos)
            };

#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return fileInfo;
        }
        public static FileInfo GetFileInfo(string filePath, IEnumerable<SearchedTypeInfo> searchedTypes, Func<List<FoundTypeInfo>, List<Log>> logsCreator)
        {
#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetFileInfo));
#endif

            var searchedTypesDict = new Dictionary<int, SearchedTypeInfo>();

            int counter = 0;
            foreach (SearchedTypeInfo type in searchedTypes)
            {
                searchedTypesDict.Add(counter++, type);
            }

            FileInfo fileInfo = GetFileInfo(filePath, searchedTypesDict, logsCreator);

#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return fileInfo;
        }

        public static List<FoundTypeInfo> TryFindTypeInfos(string filePath, Dictionary<int, SearchedTypeInfo> searchedTypes)
        {
#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.BeginSample(nameof(TryFindTypeInfos));
#endif

            using var fileReader = new StreamReader(filePath);

            var memberInfos = new Dictionary<int, List<SearchedMemberInfo>>();
            var foundTypes = new Dictionary<int, FoundTypeInfo>();

            string previousLine = "";
            string line;
            int lineNumber = 1;

            while ((line = fileReader.ReadLine()) != null)
            {
                InspectLine(line, previousLine, lineNumber, filePath, searchedTypes, memberInfos, foundTypes);

                previousLine = line;
                lineNumber++;
            }

            List<FoundTypeInfo> typeInfos = foundTypes.Select(pair => pair.Value).ToList();

#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.EndSample();
#endif

            return typeInfos;
        }

        public static void InspectLine(string line, string previousLine, int lineNumber, string filePath,
            Dictionary<int, SearchedTypeInfo> searchedTypes, Dictionary<int, List<SearchedMemberInfo>> memberInfos,
            Dictionary<int, FoundTypeInfo> foundTypes)
        {
#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.BeginSample(nameof(InspectLine));
#endif

            if (previousLine.Contains("NeedsRefactor") || line.Contains("NeedsRefactor"))
            {
                InspectLineForTitles(line, lineNumber, filePath, searchedTypes, memberInfos, foundTypes);
                InspectLineForMembers(line, lineNumber, memberInfos, foundTypes);
            }

#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.EndSample();
#endif
        }

        public static void InspectLineForTitles(string line, int lineNumber, string filePath,
            Dictionary<int, SearchedTypeInfo> searchedTypes, Dictionary<int, List<SearchedMemberInfo>> memberInfos,
            Dictionary<int, FoundTypeInfo> foundTypes)
        {
#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.BeginSample(nameof(InspectLineForTitles));
#endif

            List<int> needToRemoveFromSearchedTypes = new List<int>();

            foreach ((int index, SearchedTypeInfo info) in searchedTypes)
            {
                int columnNumber = line.IndexOf(info.titleInfo.type.GetTextToSearch());
                if (columnNumber != -1)
                {
                    memberInfos.Add(index, info.memberInfos);
                    foundTypes.Add(index, new FoundTypeInfo
                    {
                        titleInfo = new FoundTitleInfo
                        {
                            titleInfo = info.titleInfo,
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

#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.EndSample();
#endif
        }

        public static void InspectLineForMembers(string line, int lineNumber,
            Dictionary<int, List<SearchedMemberInfo>> memberInfos, Dictionary<int, FoundTypeInfo> foundTypes)
        {
#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.BeginSample(nameof(InspectLineForMembers));
#endif

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
                            memberInfo = info,
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

#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.EndSample();
#endif
        }

        internal static string GetTextToSearch(this Type type)
        {
#if FILE_TYPE_SEARCHER_PROFILING
            Profiler.BeginSample(nameof(GetTextToSearch));
#endif

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
#if FILE_TYPE_SEARCHER_PROFILING
                Profiler.EndSample();
#endif

                return $"class {typeName}";
            }
            else if (type.IsInterface)
            {
#if FILE_TYPE_SEARCHER_PROFILING
                Profiler.EndSample();
#endif

                return $"interface {typeName}";
            }
            else if (type.IsEnum)
            {
#if FILE_TYPE_SEARCHER_PROFILING
                Profiler.EndSample();
#endif

                return $"enum {typeName}";
            }
            else
            {
#if FILE_TYPE_SEARCHER_PROFILING
                Profiler.EndSample();
#endif

                return $"struct {typeName}";
            }
        }
    }
}