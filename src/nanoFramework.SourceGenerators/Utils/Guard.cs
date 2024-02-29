using System;
using System.Diagnostics;

namespace nanoFramework.SourceGenerators.Utils
{
    internal static class Guard
    {
        [DebuggerHidden]
        public static void ThrowIfNull<T>(T parameter, string paramName)
            where T : class
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        [DebuggerHidden]
        public static void ThrowIfNullOrEmpty(string parameter, string paramName)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (parameter.Length == 0)
            {
                throw new ArgumentException("Parameter is empty.", paramName);
            }
        }

        [DebuggerHidden]
        public static void ThrowIfNullOrWhitespace(string parameter, string paramName)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (string.IsNullOrWhiteSpace(parameter))
            {
                throw new ArgumentException("Parameter is whitespace.", paramName);
            }
        }
    }
}
