using System;
using System.Collections.Generic;

namespace Biosearcher.Common.States
{
    public abstract class StateManager<TEnum> : IDisposable where TEnum : Enum
    {
        protected Dictionary<TEnum, State> _states = new Dictionary<TEnum, State>();
        internal Action<TEnum> _onStateChange;

        public State Active => _states[ActiveName];
        protected internal TEnum ActiveName { get; protected set; }
        public readonly StateHook<TEnum> Hook;

        public StateManager() => Hook = new StateHook<TEnum>(this);

        public State Register(TEnum stateName) => _states[stateName] = new State();

        protected void TryChange(TEnum stateName)
        {
            if (!ActiveName.Equals(stateName))
            {
                ActiveName = stateName;
                _onStateChange?.Invoke(stateName);
            }
        }

        public virtual void Dispose()
        {
            foreach ((_, State state) in _states)
            {
                state.Dispose();
            }
            _states.Clear();
        }
    }

    public sealed class ChangeableStateManager<TEnum> : StateManager<TEnum> where TEnum : Enum
    {
        public new void TryChange(TEnum stateName) => base.TryChange(stateName);
    }

    public sealed class HookableStateManager<TEnum> : StateManager<TEnum> where TEnum : Enum
    {
        private List<Action<TEnum>> _hookedActions = new List<Action<TEnum>>();

        public void HookTo(StateHook<TEnum> hook) => HookTo(hook.StateManager);
        public void HookTo(StateManager<TEnum> originalStateManager)
        {
            originalStateManager._onStateChange += TryChange;
            TryChange(originalStateManager.ActiveName);
        }

        public override void Dispose()
        {
            _hookedActions.Foreach(action => action -= TryChange);
            _hookedActions.Clear();
            base.Dispose();
        }
    }

    public class StateHook<TEnum> where TEnum : Enum
    {
        internal StateManager<TEnum> StateManager;

        internal StateHook(StateManager<TEnum> stateManager) => StateManager = stateManager;
    }
}
