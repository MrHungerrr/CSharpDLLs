using System;
using System.Collections.Generic;

namespace Vkimow.Unity.GOAP
{
    public class GOAPStateContext
    {
        private static GOAPStateStorage _globalState;
        private GOAPStateStorage _localState;

        public GOAPStateContext(GOAPStateStorage localState)
        {
            _localState = localState;
        }

        public static void SetGlobalState(GOAPStateStorage state)
        {
            _globalState = state;
        }

        public bool Contains(KeyValuePair<string, GOAPState> state)
        {
            if (_globalState.Contains(state))
                return true;

            return _localState.Contains(state);
        }


        public void Set(KeyValuePair<string, GOAPState> state)
        {
            if (_localState.Contains(state.Key))
            {
                _localState.Set(state);
                return;
            }

            if (_globalState.Contains(state.Key))
            {
                _globalState.Set(state);
                return;
            }

            throw new ArgumentException($"Отсутсвует GOAPState с Key:\"{state.Key}\"");
        }
    }
}
