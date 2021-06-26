#if UNITY_EDITOR

namespace Biosearcher.Refactoring
{
    internal static class RefactorSettingsExtensions
    {
        internal static RefactorSettings.Parameters GetParamsSafe(this RefactorSettings settings)
        {
            if (settings == null)
            {
                return new RefactorSettings.Parameters
                {
                    enabled = true,
                    showInConsole = true
                };
            }
            return settings.Params;
        }
    }
}

#endif
