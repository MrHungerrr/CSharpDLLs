using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Vkimow.Unity.GOAP
{
    public class GOAPStateSingle : IGOAPStateSingle
    {
        public bool IsEmpty => !_state.HasValue;

        private KeyValuePair<string, GOAPState>? _state;



        #region Set

        public void Set(string key, object value)
        {
            Set(key, new GOAPState(value));
        }

        public void Set(string key, GOAPState state)
        {
            Set(new KeyValuePair<string, GOAPState>(key, state));
        }

        public void Set(KeyValuePair<string, GOAPState> state)
        {
            if (!GOAPBlanksManager.Instance.Contains(state.Key))
                throw new Exception($"Нет элемента с ключом {state.Key}");

            if (GOAPBlanksManager.Instance.Blanks[state.Key] != state.Value.Type)
                throw new Exception($"Нет элемента \"{state.Key}\" с типом {state.Value.Type}");

            _state = state;
        }
        #endregion


        public void Clear()
        {
            _state = null;
        }


        #region Contains
        public bool Contains(string key)
        {
            if (!_state.HasValue)
                return false;

            return _state.Value.Key == key;
        }

        public bool Contains(string key, object value)
        {
            if (!_state.HasValue)
                return false;

            if (_state.Value.Key != key)
                return false;

            return _state.Value.Value.ValueEquals(value);
        }

        public bool Contains(string key, GOAPState state)
        {
            if (!_state.HasValue)
                return false;

            if (_state.Value.Key != key)
                return false;

            return _state.Value.Value.Equals(state);
        }

        public bool Contains(KeyValuePair<string, GOAPState> state)
        {
            if (!_state.HasValue)
                return false;

            if (_state.Value.Key != state.Key)
                return false;

            return _state.Value.Value.Equals(state.Value);
        }
        #endregion


        public override string ToString()
        {
            return $"\"{_state.Value.Key}\" = {_state.Value.Value.Value}";
        }

        #region XML Serialization

        public XElement ConvertToXML()
        {
            return ConvertToXML("GOAPState_Single");
        }

        public XElement ConvertToXML(string name)
        {
            var xElement = new XElement(name, new XAttribute("IsEmpty", IsEmpty));
            
            if(!IsEmpty)
            {
                xElement.Add(new XElement("Key", _state.Value.Key), new XElement("Value", _state.Value.Value.Value));
            }

            return xElement;
        }

        public void ReadXML(XElement xElement)
        {
            if (((bool)xElement.Attribute("IsEmpty")))
                return;

            var key = (string)xElement.Element("Key");
            object value;
            
            switch(GOAPBlanksManager.Instance.Blanks[key].ToString())
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

            Set(key, value);
        }

        #endregion
    }
}
