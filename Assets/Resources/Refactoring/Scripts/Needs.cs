using System;

namespace Biosearcher.Refactoring
{
    [Flags]
    public enum Needs
    {
        Refactor = 0,

        Reformat = 1,
        Optimization = 2,
        Review = 4,
        Remove = 8,
        Implementation = 16,
        Rename = 32,
        RemoveTodo = 64,
        MakeOwnFile = 128,
    }
}