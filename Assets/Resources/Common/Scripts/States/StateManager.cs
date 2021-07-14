using System;
using System.Collections.Generic;

namespace Biosearcher.Common.States
{
    public sealed class StateManager<TEnum> : IDisposable where TEnum : Enum
    {
        private Dictionary<TEnum, State> _states = new Dictionary<TEnum, State>();

        public State Active { get; private set; }

        public State Register(TEnum stateName) => _states[stateName] = new State();

        public void Change(TEnum stateName) => Active = _states[stateName];

        public void Dispose()
        {
            foreach((_, State state) in _states)
            {
                state.Dispose();
            }
            _states.Clear();
        }
    }
}
