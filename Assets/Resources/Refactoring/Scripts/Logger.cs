//#define LOGGER_PROFILING

using Biosearcher.Common;
using Biosearcher.Refactoring.FileInput;
using System;
using System.Collections.Generic;
using UnityEngine;
#if LOGGER_PROFILING
using UnityEngine.Profiling;
#endif

namespace Biosearcher.Refactoring
{
    internal static class Logger
    {
        internal static List<Log> ToLogs(List<FoundTypeInfo> foundTypeInfos)
        {
#if LOGGER_PROFILING
            Profiler.BeginSample(nameof(ToLogs));
#endif

            var logs = new List<Log>();
            foreach (FoundTypeInfo info in foundTypeInfos)
            {
                logs.Add(
                    new Log(
                        GetLogText(info.titleInfo.titleInfo.type, info.titleInfo.titleInfo.attribute),
                        info.filePath,
                        info.titleInfo.titleInfo.type.Namespace,
                        info.titleInfo.lineNumber,
                        info.titleInfo.columnNumber));
                info.memberInfos
                    .Foreach(member => logs.Add(
                        new Log(
                            GetLogText(info.titleInfo.titleInfo.type, member.memberInfo.member.Name, member.memberInfo.attribute),
                            info.filePath,
                            info.titleInfo.titleInfo.type.Namespace,
                            member.lineNumber,
                            member.columnNumber)));
            }

#if LOGGER_PROFILING
            Profiler.EndSample();
#endif

            return logs;
        }

        internal static string GetLogText(Type type, NeedsRefactorAttribute attribute)
        {
            return $"<b><color=#99ccff>{type.Namespace}</color>. <color=#99ff99>{type.Name}</color></b> needs <color=#ff6699>{attribute.NeededAction}</color>.";
        }
        internal static string GetLogText(Type type, string memberName, NeedsRefactorAttribute attribute)
        {
            return $"<b><color=#99ccff>{type.Namespace}</color>. <color=#99ff99>{type.Name}</color>. " +
                $"<color=#ffff99>{memberName}</color></b> needs <color=#ff6699>{attribute.NeededAction}</color>.";
        }

        internal static void LogAll(IEnumerable<Log> logs)
        {
#if LOGGER_PROFILING
            Profiler.BeginSample(nameof(LogAll));
#endif

            logs.Foreach(log => Debug.LogWarningFormat(log.Text));

#if LOGGER_PROFILING
            Profiler.EndSample();
#endif
        }
    }
}