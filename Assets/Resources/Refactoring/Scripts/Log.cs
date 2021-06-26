namespace Biosearcher.Refactoring
{
    public readonly struct Log
    {
        public string Text { get; }
        public string FilePath { get; }
        public string Namespace { get; }
        public int LineNumber { get; }
        public int ColumnNumber { get; }

        public Log(string text, string filePath, string @namespace, int lineNumber = 0, int columnNumber = 0)
        {
            Text = text;
            FilePath = filePath;
            Namespace = @namespace;
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
        }
    }
}