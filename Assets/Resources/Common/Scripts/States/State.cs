using System;
using System.Collections.Generic;

namespace Biosearcher.Common.States
{
    public sealed class State : IDisposable
    {
        private Dictionary<object, object> _actions = new Dictionary<object, object>();

        public State Register<T>(T method, T action) where T : Delegate
        {
            _actions[method] = action;
            return this;
        }
        public T Get<T>(T method) where T : Delegate => (T)_actions[method];

        public State Register(Action method, Action action) => Register<Action>(method, action);
        public State Register<T>(Action<T> method, Action<T> action) => Register<Action<T>>(method, action);
        public State Register<T1, T2>(Action<T1, T2> method, Action<T1, T2> action) => Register<Action<T1, T2>>(method, action);
        public State Register<T1, T2, T3>(Action<T1, T2, T3> method, Action<T1, T2, T3> action) => Register<Action<T1, T2, T3>>(method, action);
        public State Register<R>(Func<R> method, Func<R> action) => Register<Func<R>>(method, action);
        public State Register<T, R>(Func<T, R> method, Func<T, R> action) => Register<Func<T, R>>(method, action);
        public State Register<T1, T2, R>(Func<T1, T2, R> method, Func<T1, T2, R> action) => Register<Func<T1, T2, R>>(method, action);
        public State Register<T1, T2, T3, R>(Func<T1, T2, T3, R> method, Func<T1, T2, T3, R> action) => Register<Func<T1, T2, T3, R>>(method, action);

        public Action Get(Action method) => Get<Action>(method);
        public Action<T> Get<T>(Action<T> method) => Get<Action<T>>(method);
        public Action<T1, T2> Get<T1, T2>(Action<T1, T2> method) => Get<Action<T1, T2>>(method);
        public Action<T1, T2, T3> Get<T1, T2, T3>(Action<T1, T2, T3> method) => Get<Action<T1, T2, T3>>(method);
        public Func<R> Get<R>(Func<R> method) => Get<Func<R>>(method);
        public Func<T, R> Get<T, R>(Func<T, R> method) => Get<Func<T, R>>(method);
        public Func<T1, T2, R> Get<T1, T2, R>(Func<T1, T2, R> method) => Get<Func<T1, T2, R>>(method);
        public Func<T1, T2, T3, R> Get<T1, T2, T3, R>(Func<T1, T2, T3, R> method) => Get<Func<T1, T2, T3, R>>(method);

        public void Invoke(Action method) => ((Action)_actions[method])?.Invoke();
        public void Invoke<T>(Action<T> method, T param) => Get<Action<T>>(method)?.Invoke(param);
        public void Invoke<T1, T2>(Action<T1, T2> method, T1 param1, T2 param2) => ((Action<T1, T2>)_actions[method])?.Invoke(param1, param2);
        public void Invoke<T1, T2, T3>(Action<T1, T2, T3> method, T1 param1, T2 param2, T3 param3) => ((Action<T1, T2, T3>)_actions[method])?.Invoke(param1, param2, param3);
        public R Invoke<R>(Func<R> method) => ((Func<R>)_actions[method]).InvokeOrDefault();
        public R Invoke<T, R>(Func<T, R> method, T param) => Get<Func<T, R>>(method).InvokeOrDefault(param);
        public R Invoke<T1, T2, R>(Func<T1, T2, R> method, T1 param1, T2 param2) => ((Func<T1, T2, R>)_actions[method]).InvokeOrDefault(param1, param2);
        public R Invoke<T1, T2, T3, R>(Func<T1, T2, T3, R> method, T1 param1, T2 param2, T3 param3) => ((Func<T1, T2, T3, R>)_actions[method]).InvokeOrDefault(param1, param2, param3);

        public void Dispose() => _actions.Clear();
    }
}
