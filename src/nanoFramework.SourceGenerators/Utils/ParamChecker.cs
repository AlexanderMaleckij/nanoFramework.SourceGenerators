using System.Diagnostics;

namespace nanoFramework.SourceGenerators.Utils
{
    internal static class ParamChecker
    {
        [DebuggerHidden]
        public static T Check<T>(T parameter, string parameterName)
            where T : class
        {
            Guard.ThrowIfNull(parameter, parameterName);

            return parameter;
        }
    }
}
