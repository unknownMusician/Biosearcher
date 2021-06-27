using System;

namespace Biosearcher.Refactoring
{
    [Flags]
    public enum Needs
    {
        Refactor            = 0,

        Reformat            = 1 << 0,
        Optimization        = 1 << 1,
        AddRegions          = 1 << 2,
        Remove              = 1 << 3,
        Implementation      = 1 << 4,
        Rename              = 1 << 5,
        RemoveTodo          = 1 << 6,
        MakeOwnFile         = 1 << 7,
    }
}