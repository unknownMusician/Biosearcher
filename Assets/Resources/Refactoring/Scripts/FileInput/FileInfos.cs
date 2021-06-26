using System;
using System.Collections.Generic;
using System.Reflection;

namespace Biosearcher.Refactoring.FileInput
{
    [NeedsRefactor]
    public struct FileInfo
    {
        public DateTime writeTime;
        public List<FoundTypeInfo> typeInfos;
        public List<Log> logs;
    }

    [NeedsRefactor]
    public struct SearchedTypeInfo
    {
        public SearchedTitleInfo titleInfo;
        public List<SearchedMemberInfo> memberInfos;
    }

    [NeedsRefactor]
    public struct FoundTypeInfo
    {
        public FoundTitleInfo titleInfo;
        public List<FoundMemberInfo> memberInfos;
        public string filePath;
    }

    [NeedsRefactor]
    public struct SearchedTitleInfo
    {
        public Type type;
        public NeedsRefactorAttribute attribute;
    }

    [NeedsRefactor]
    public struct FoundTitleInfo
    {
        public SearchedTitleInfo titleInfo;
        public int lineNumber;
        public int columnNumber;
    }

    [NeedsRefactor]
    public struct SearchedMemberInfo
    {
        public MemberInfo member;
        public NeedsRefactorAttribute attribute;
    }

    [NeedsRefactor]
    public struct FoundMemberInfo
    {
        public SearchedMemberInfo memberInfo;
        public int lineNumber;
        public int columnNumber;
    }
}