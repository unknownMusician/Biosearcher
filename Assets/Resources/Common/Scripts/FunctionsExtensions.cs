using System;

namespace Biosearcher.Common
{
    public static class FunctionsExtensions
    {
        public static R InvokeOrDefault<R>(this Func<R> func) => func is null ? default : func.Invoke();
        public static R InvokeOrDefault<T, R>(this Func<T, R> func, T param) => func is null ? default : func.Invoke(param);
        public static R InvokeOrDefault<T1, T2, R>(this Func<T1, T2, R> func, T1 param1, T2 param2)
            => func is null ? default : func.Invoke(param1, param2);
        public static R InvokeOrDefault<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T1 param1, T2 param2, T3 param3)
            => func is null ? default : func.Invoke(param1, param2, param3);
        public static R InvokeOrDefault<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> func, T1 param1, T2 param2, T3 param3, T4 param4)
            => func is null ? default : func.Invoke(param1, param2, param3, param4);

    }
}