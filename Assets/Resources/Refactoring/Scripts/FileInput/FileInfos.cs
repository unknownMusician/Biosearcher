using System;
using System.Collections.Generic;
using System.Reflection;

namespace Biosearcher.Refactoring.FileInput
{
    public struct FileInfo
    {
        public DateTime writeTime;
        public List<FoundTypeInfo> typeInfos;
        public List<Log> logs;
    }

    public struct SearchedTypeInfo
    {
        public SearchedTitleInfo titleInfo;
        public List<SearchedMemberInfo> memberInfos;
    }

    public struct FoundTypeInfo
    {
        public FoundTitleInfo titleInfo;
        public List<FoundMemberInfo> memberInfos;
        public string filePath;
    }

    public struct SearchedTitleInfo
    {
        public Type type;
        public NeedsRefactorAttribute attribute;
    }

    public struct FoundTitleInfo
    {
        public SearchedTitleInfo titleInfo;
        public int lineNumber;
        public int columnNumber;
    }

    public struct SearchedMemberInfo
    {
        public MemberInfo member;
        public NeedsRefactorAttribute attribute;
    }

    public struct FoundMemberInfo
    {
        public SearchedMemberInfo memberInfo;
        public int lineNumber;
        public int columnNumber;
    }
}