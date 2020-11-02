using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Vkimow.Unity.GOAP
{
    public class GOAPStateStorage : IGOAPStateStorage
    {
        public bool IsEmpty => _states.Count == 0;

        private Dictionary<string, GOAPState> _states;

        public GOAPStateStorage()
        {
            _states = new Dictionary<string, GOAPState>();
        }

        public GOAPStateStorage(IEnumerable<KeyValuePair<string, GOAPState>> states)
        {
            _states = new Dictionary<string, GOAPState>(states.ToDictionary((x) => x.Key, (x) => x.Value));
        }


        #region Logic

        #region Add & Remove

        public void Add(string key)
        {
            Add(key, true);
        }

        public void Add(string key, object value)
        {
            var goapState = new GOAPState(value);
            Add(key, goapState);
        }

        public void Add(string key, GOAPState state)
        {
            if (!GOAPBlanksManager.Instance.Contains(key))
                throw new Exception($"Нет элемента с ключом {key}");

            if(GOAPBlanksManager.Instance.Blanks[key] != state.Type)
                throw new Exception($"Нет элемента \"{key}\" с типом {state.Type}");

            _states.Add(key, state);
        }

        public void Add(KeyValuePair<string, GOAPState> state)
        {
            Add(state.Key, state.Value);
        }

        public void Remove(string key)
        {
            _states.Remove(key);
        }

        public void Clear()
        {
            _states = new Dictionary<string, GOAPState>();
        }
        #endregion

        #region Contains
        public bool Contains(string key)
        {
            return _states.ContainsKey(key);
        }

        public bool Contains(string key, object value)
        {
            if (!_states.ContainsKey(key))
                return false;

            return _states[key].ValueEquals(value);
        }

        public bool Contains(string key, GOAPState state)
        {
            if (!_states.ContainsKey(key))
                return false;

            return _states[key].Equals(state);
        }

        public bool Contains(KeyValuePair<string, GOAPState> state)
        {
            if (!_states.ContainsKey(state.Key))
                return false;

            return _states[state.Key].Equals(state.Value);
        }
        #endregion

        #region Set

        public void Set(string key, object value)
        {
            Set(key, new GOAPState(value));
        }

        public void Set(string key, GOAPState state)
        {
            _states[key].SetValue(state);
        }

        public void Set(KeyValuePair<string, GOAPState> state)
        {
            Set(state.Key, state.Value);
        }
        #endregion

        #endregion

        #region IEnumerator & Indexer

        public IEnumerator<KeyValuePair<string, GOAPState>> GetEnumerator()
        {
            return _states.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _states.GetEnumerator();
        }

        public GOAPState this[string key]
        {
            get
            {
                return _states[key];
            }
        }

        #endregion


        public override string ToString()
        {
            string text = string.Empty;

            foreach(var state in _states)
            {
                if(text == string.Empty)
                    text += $"\"{state.Key}\" = {state.Value.Value}";
                else
                    text += $", \"{state.Key}\" = {state.Value.Value}";
            }

            return text;
        }


        #region XML Serialization

        public XElement ConvertToXML()
        {
            return ConvertToXML("GOAPState_Storage");
        }

        public XElement ConvertToXML(string name)
        {
            var xElement = new XElement(name, new XAttribute("IsEmpty", IsEmpty));

            if (!IsEmpty)
            {
                foreach (var state in _states)
                {
                    var xKeyValue = new XElement("State", new XElement("Key", state.Key), new XElement("Value", state.Value.Value));
                    xElement.Add(xKeyValue);
                }
            }

            return xElement;
        }

        public void ReadXML(XElement xMainElement)
        {
            if (((bool)xMainElement.Attribute("IsEmpty")))
                return;


            foreach (var xElement in xMainElement.Elements())
            {
                var key = (string)xElement.Element("Key");
                object value;

                switch (GOAPBlanksManager.Instance.Blanks[key].ToString())
                {
                    case "System.Boolean":
                        {
                            value = (bool)xElement.Element("Value");
                            break;
                        }
                    case "System.String":
                        {
                            value = (string)xElement.Element("Value");
                            break;
                        }
                    case "System.Int32":
                        {
                            value = (int)xElement.Element("Value");
                            break;
                        }
                    default:
                        {
                            throw new InvalidCastException();
                        }
                }

                Add(key, value);
            }
        }

        #endregion
    }
}
